var TABLE = process.env.DATABASE_TABLE;
var mysql = require('mysql2');
var database = mysql.createPool({
    host: process.env.DATABASE_HOST,
    user: process.env.DATABASE_USER,
    password: process.env.DATABASE_PASSWORD,
    database: process.env.DATABASE_NAME,
    waitForConnections: true,
    connectionLimit: 10,
    queueLimit: 0
});

const createTableQuery = `
    CREATE TABLE IF NOT EXISTS ${TABLE} (
    id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,

    username VARCHAR(255) DEFAULT NULL UNIQUE,
    password VARCHAR(255) DEFAULT NULL,
    licensekey VARCHAR(32) NOT NULL UNIQUE DEFAULT (MD5(UUID())),
    licenseExpiration DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    licenseSuspended BOOL NOT NULL DEFAULT FALSE,
    hardwareIds LONGTEXT DEFAULT '[]',
    restrictions LONGTEXT DEFAULT '{}'
);`;

// restrictions LONGTEXT DEFAULT '{}'
// IP_ADDRESS:"127.0.0.1"
// DISCORD_ID:"123" (required discord to be running, otherwise always fail)

// ACCOUNT_LIMIT:3
// CHARACTER_SERVER:["pl34"]
// CHARACTER_GUILD:821 (guild id lock)

//alerts LONGTEXT DEFAULT '[]'
//"EJ PRZESTAN LAGOWAC SERWER"

const createDeveloperQuery = `
    INSERT IGNORE INTO ${TABLE} (username, password, hardwareIds, restrictions)
    VALUES (?, ?, ?, ?);
`

database.getConnection((err, connection) => {
    if (err) {
        throw new Error('Unable to establish connection to database! ' + err);
    }

    connection.query(createTableQuery, (err, results) => {
        connection.release();
        if (err) {
            console.error('Error creating table:', err, results);
        }
    });

    /*connection.query(createDeveloperQuery, ['developer', '$2b$10$URUiuCRP2n08j/eW70I0qOhqSSAmgjvPcg3z3Q4GQNJ/wpBRsue4W', '["*"]', '{"IP_ADDRESS":"::ffff:127.0.0.1"}'], (err, results) => {
        connection.release();

        if (err) {
            console.error('Error inserting user:', err);
            return;
        }

        console.log('New user created');
    });*/
});

setInterval(() => {
    database.getConnection((err, connection) => {
        if (err) {
            console.error('Error pinging database connection:', err);
            return;
        }

        connection.ping((pingErr) => {
            connection.release();
        });
    });
}, 30000);

module.exports = {
    database,
    TABLE
};
