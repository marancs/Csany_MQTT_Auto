const util = require('util');
const express = require('express');
const session = require('express-session');
const app    = express();
const port   = 3000;
const mqtt = require('mqtt')
var DB      = require('./data.js');

var mqtt_Client = `mqtt_${Math.random().toString(16).slice(3)}`;
var mqtt_Url = `mqtt://${process.env.MQTT_HOST}:${process.env.MQTT_PORT}`
var mqtt_Username = process.env.mqtt_usr; // mqtt credentials if these are needed to connect
var mqtt_Password = process.env.mqtt_pass;

app.use(express.static('public'));    // frontend root mappa (index.html)?

//MQTT
mqtt_Client = mqtt.connect(mqtt_Url, {
    username: mqtt_Username,
    password: mqtt_Password,
});

mqtt_Client.on("error", (err) => {
    console.log(err);
    mqtt_Client.end();
  });

mqtt_Client.on("connect", () => {
    console.log(`mqtt client connected`);
  });

mqtt_Client.subscribe("Auto/Keres",{ qos: 0 })

function GetAllAuto(){
    SendToJsonAsync("SELECT * FROM Auto").then(
        adatok=>{
                console.log("Adatok továbbitása");
                mqtt_Client.publish("Auto/Keres",adatok);
            }
    )
}

// receive a message from the subscribed topic
mqtt_Client.on('message',(topic, message) => {
    //console.log(`message: ${message}, topic: ${topic}`); 
    if (topic=="Auto/Keres" && message == "all"){
        console.log("Összes auto lekérdezése..")
        GetAllAuto();
    }
});

const SendToJsonAsync =function (sql) {
    return new Promise((resolve, reject) => {
        DB.query(sql, null ,(json_data, error) => {
            let data = error ? error : JSON.parse( JSON.stringify( json_data ));
            if (error) {  console.log(error)}
            if (!error && data.length > 0)  {
                adatok= JSON.stringify(JSON.parse(data).dataset);
                resolve(adatok);
            }else{
                reject(error);
            }
        });
   })
}
function Send_to_JSON (req, res, sql) {
    DB.query(sql, naplo(req), (json_data, error) => {
      let data = error ? error : JSON.parse( json_data ); 
      res.set('Content-Type', 'application/json; charset=UTF-8');
      res.send(data);
      res.end();
    });
  }
/* ---------------------------- log 'fájl' naplózás ------------------  */
function naplo (req) {
    var userx = "- no login -";
    session_data = req.session;
    if (session_data.ID_USER) {  userx = session_data.EMAIL; } 
    return [ userx, req.socket.remoteAddress ];
  }

//HTML POST GET

app.post('/tabla', (req, res) => {  /* ---------- tábla ----------- */
    var sql = 
      `SELECT ID_TERMEK, NEV, k.KATEGORIA AS KATEGORIA, AR, MENNYISEG, MEEGYS
       FROM IT_termekek t INNER JOIN IT_kategoriak k 
       ON t.ID_KATEGORIA = k.ID_KATEGORIA 
       Where t.ID_KATEGORIA = 1
       ORDER BY NEV
       LIMIT 10 offset 0;`
    Send_to_JSON(req, res, sql);
});

app.post('/kategoria', (req, res) => {  /* ---------- kategoria listadoboz ----------- */
    var sql = "SELECT ID_KATEGORIA, KATEGORIA from IT_kategoriak order by KATEGORIA";
    Send_to_JSON(req, res, sql);
});

app.post('/logout',   (req, res) => {  
    session_data = req.session;
    session_data.destroy(function(err) {
        res.set('Content-Type', 'application/json; charset=UTF-8');
        res.json('Session destroyed');
        res.end();
      }); 
  });
  
  app.post('/login',  (req, res) => {
    var user= (req.query.user1_login_field? req.query.user1_login_field: "");
    var psw = (req.query.user1_passwd_field? req.query.user1_passwd_field  : "");
    var sql = `select ID_USER, NEV, EMAIL from userek where EMAIL="${user}" and PASSWORD=md5("${psw}") limit 1;`;
   
    DB.query(sql, napló(req), (json_data, error) => {
      var data = error ? error : JSON.parse( json_data ); 
      console.log(util.inspect(data, false, null, true ));     // obj. full. kiírása
      
      if (!error && data.count == 1)  {  /* rövidzár kiért. : sikeres bejelentkezés, megvan a juzer... */                    
          session_data = req.session;
          session_data.ID_USER = data.rows[0].ID_USER;
          session_data.EMAIL   = data.rows[0].EMAIL;
          session_data.NEV     = data.rows[0].NEV;
          session_data.MOST    = Date.now();
          console.log("Setting session data:username=%s and id_user=%s", session_data.NEV, session_data.ID_USER);
      }
  
      res.set('Content-Type', 'application/json; charset=UTF-8');
      res.send(data);
      res.end();
    });
  });

app.listen(port, function () { console.log(`Example app listening at http://localhost:${port}`); });
