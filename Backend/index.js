const util = require('util');
const app    = express();
const port   = 3000;
const mqtt = require('mqtt')
var DB      = require('./data.js');
var mqtt_Client = `mqtt_${Math.random().toString(16).slice(3)}`;
var mqtt_Url = `mqtt://${process.env.MQTT_HOST}:${process.env.MQTT_PORT}`
var mqtt_Username = process.env.mqtt_usr; // mqtt credentials if these are needed to connect
var mqtt_Password = process.env.mqtt_pass;


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

app.listen(port, function () { console.log(`Example app listening at http://localhost:${port}`); });
