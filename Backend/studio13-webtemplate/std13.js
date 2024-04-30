/* 
   Bootstrap5  (https://www.w3schools.com/bootstrap5/) ; 
   mysql tábla megjelenítése; katt eseményre rekord megjelenítése; popup ablakban rekord módosítás 
   SQL tokenizer
*/

const util = require('util');
const express = require('express');
const session = require('express-session');
const app    = express();
const port   = 3000;
var session_data;                   // login user adatai
app.use(session({ key:'user_sid', secret:'nagyontitkossütemény', resave:true, saveUninitialized:true }));  /* https://www.js-tutorials.com/nodejs-tutorial/nodejs-session-example-using-express-session */
var DB  = require('./datamodule_mysql.js');
app.use(express.static('public'));    // frontend root mappa (index.html)

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

/* --- mysql pool technikával, json formátumban visszaküldi a kliensnek az adathalmazt: restapi ----*/
 
 function Send_to_JSON (req, res, sql) {
  DB.query(sql, napló(req), (json_data, error) => {
    let data = error ? error : JSON.parse( json_data ); 
    res.set('Content-Type', 'application/json; charset=UTF-8');
    res.send(data);
    res.end();
  });
}


/* ---------------------------- log 'fájl' naplózás ------------------  */
function napló (req) {
  var userx = "- no login -";
  session_data = req.session;
  if (session_data.ID_USER) {  userx = session_data.EMAIL; } 
  return [ userx, req.socket.remoteAddress ];
}

app.listen(port, function () { console.log(`std13 app listening at http://localhost:${port}`); });