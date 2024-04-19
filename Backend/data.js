var mysql = require("mysql");
var dotenv = require("dotenv");
dotenv.config();

const pool = mysql.createPool({
    host:     process.env.MYSQL_HOST,
    user:     process.env.MYSQL_USR,
    password: process.env.MYSQL_PASS,
    database: process.env.MYSQL_DB,
    port:     process.env.MYSQL_PORT,    
    connectionLimit : 10
});

const getConnection =function () {
    return new Promise((resolve, reject) => {
        pool.getConnection((error, con) => {
            if (error)  {               
                reject( error);
                console.log(11);
            }
            else
                resolve(con);
        });
    });
}


var DB = (function () {
    function _query(sql, params, callback) {
        getConnection()
        .then((connection)=>{
            var worlds = sql.trim().toUpperCase().split(" ");
            var isSelect = worlds[0] === "SELECT";
            var limit_poz = worlds.lastIndexOf("LIMIT");             /* ORDER BY után kell csak keresni */
            var limit = limit_poz > -1? worlds[limit_poz+1]*1 : -1;  /* order by x limit 100 offset 200 */
            var offset_poz = worlds.lastIndexOf("OFFSET");
            var offset = offset_poz > -1? ((worlds[offset_poz+1]*1) / limit) | 0 : -1; 
            var maxcount = 0; 
            if (isSelect && limit > 0) {                                          /* maxcount keresése (limit nélkül mennyi lenne?) */
                let poz = sql.toUpperCase().lastIndexOf("ORDER BY ");             // nem kell az order by (ha van)
                if (poz < 0) { poz = sql.toUpperCase().lastIndexOf("LIMIT "); }   // nem kell a limit (ha van)
                let sqlcount = "select count(*) as db from ("+sql.substring(0, poz)+") as tabla;"; 
                connection.query(sqlcount, params, function (err, rows) { 
                    rows = JSON.parse(JSON.stringify(rows));
                    maxcount = rows[0].db; 
                });    
            }
            connection.query(sql, params, function (err, rows) {
                connection.release();
                var js;
                if (!err) { 
                    if (isSelect) {                                            /* select .... */        
                        let reccount = rows.length;
                        let pages = Math.floor(maxcount / limit) + (maxcount % limit >= 1 ? 1:0);
                        js = { "message"  : 0, 
                               "tip"      : reccount > 0? "info" : "alert", 
                               "count"    : reccount,         /* limit reccount   */ 
                               "maxcount" : maxcount,         /* no limit reccount */
                               "limit"    : limit,            /* akt.  limit */
                               "offset"   : offset,           /* akt. offset */
                               "pages"    : pages,            /* max. lapok száma */ 
                               "dataset"  : rows }      
                    } else  { 
                        const templ = { "INSERT": "Bevitel", "UPDATE": "Módosítás", "DELETE": "Törlés"}; 
                        js = rows ;                           /* insert, update, delete (+DDLsql) {fieldCount:, affectedRows:,insertId:,serverStatus:,warningCount:,message:,protocol41:,changedRows:} */
                        js["count"]   = js["affectedRows"];
                        js["tip"]     = js["count"]== 0? "alert": "info"; 
                        js["text"] = templ[worlds[0]]+": "+js["count"]+" rekord." ;   
                    }        
                    callback (JSON.stringify( js )); 
                }
                else { 
                    js = { "text" : "["+err.errno+"] " + err.sqlMessage + " --> " + err.sqlMessage,  "tip" : "error"};
                    callback( null, JSON.stringify( js ));
                }
            });
            connection.on('error', function (err) {
                console.log(7)
                connection.release();
                callback(null, JSON.stringify(err));
            });
        })
        .catch((err)=>{callback(null, JSON.stringify(err));  })
    }
    
    return { query: _query };
}) ();

module.exports = DB;