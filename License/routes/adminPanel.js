var express = require('express');
var path = require('path');
var router = express.Router();

function isLocalRequest(req) {
    const localIps = ['127.0.0.1', '::ffff:127.0.0.1', '::1'];
    return localIps.includes(req.ip) || req.connection.remoteAddress === '127.0.0.1';
}

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

router.get('/', function (req, res, next) {
    if (!isLocalRequest(req)) {
        return res.status(403).send();
    }

    res.sendFile(path.join(__dirname, '..', 'public', 'admin.html'));
});

router.get('/get-users', function (req, res) {
    const query = `SELECT id, username, discordId, ipAddress, licensekey, licenseExpiresAt, licenseStatus FROM ${TABLE}`;

    // Query the database
    database.query(query, (err, results) => {
        if (err) {
            console.error(err);
            return res.status(500).json({ error: 'Database query failed' });
        }
        res.json(results);
    });
});

router.post('/create-or-edit-user', function (req, res) {
    const { username, password, discordId, ipAddress, licenseExpiresAt, licenseStatus } = req.body;

    // Check if the username already exists
    const checkUserQuery = `SELECT * FROM ${TABLE} WHERE username = ?`;

    database.query(checkUserQuery, [username], (err, existingUser) => {
        if (err) {
            console.error(err);
            return res.status(500).json({ error: 'Database query failed' });
        }

        if (existingUser.length > 0) {
            // User exists, update their details
            const updateQuery = `
                UPDATE ${TABLE}
                SET password = ?, discordId = ?, ipAddress = ?, licenseExpiresAt = ?, licenseStatus = ?
                WHERE username = ?
            `;

            database.query(updateQuery, [password, discordId, ipAddress, licenseExpiresAt, licenseStatus, username], (err, results) => {
                if (err) {
                    console.error(err);
                    return res.status(500).json({ error: 'Database update failed' });
                }
                res.send('User updated successfully');
            });

        } else {
            // User does not exist, insert a new user
            const insertQuery = `
                INSERT INTO ${TABLE} (username, password, discordId, ipAddress, licenseExpiresAt, licenseStatus)
                VALUES (?, ?, ?, ?, ?, ?)
            `;

            database.query(insertQuery, [username, password, discordId, ipAddress, licenseExpiresAt, licenseStatus], (err, results) => {
                if (err) {
                    console.error(err);
                    return res.status(500).json({ error: 'Database insert failed' });
                }
                res.send('User created successfully');
            });
        }
    });
});

module.exports = router;
