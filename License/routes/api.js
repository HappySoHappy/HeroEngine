var express = require('express');
var router = express.Router();

var { aesDecrypt, aesEncrypt } = require('../utils/aes');
var { bcryptEncrypt, bcryptCompare } = require('../utils/bcrypt')

var { database, TABLE} = require('../utils/database')

router.post('/v1', async (req, res) => {
    if (!req.is('text')) {
        req.socket.destroy();
        return;
    }

    try {
        const decryptedBody = aesDecrypt(req.body);
        req.body = JSON.parse(decryptedBody);
    } catch {
        console.log(`\x1b[0m${req.ip} \x1b[33mPOST \x1b[0mcontains invalid encrypted body - \x1b[31m` + req.body);
        req.socket.destroy();
        return;
    }

    var auth = req.get('authorization');
    if (auth) {
        auth = auth.replace("Bearer ", "");
    }

    switch (req.body.action) {
        case "startup":

            console.log(`\x1b[0m${req.ip} \x1b[33mCLIENT_STARTUP \x1b[0m- ` + JSON.stringify(req.body));

            return;

        case "login":
            const username = req.body.user;
            const password = req.body.password;
            const hwid = req.body.hardware;
            const version = req.body.version;

            if (!username || !password || !hwid || !version) {
                res.status(403).send(aesEncrypt(JSON.stringify({ error: 'rejected' })));
                return;
            }

            const discord = req.body.discord;

            console.log(`\x1b[0m${req.ip} \x1b[33mLOGIN_REQUEST \x1b[0m- ` + JSON.stringify(req.body));

            if (!bcryptCompare((username + password + hwid + version), auth ?? '')) {
                console.log(`\x1b[0m${req.ip} \x1b[33mLOGIN_REQUEST \x1b[0mfor ` + username + ` \x1b[0mcontains fabricated authorization - \x1b[31m` + auth);
                //trigger webhook
            }

            //'    --    ;    /*
            const sqlInjectionPattern = /['"<>;=()--\/*]/;
            if (sqlInjectionPattern.test(username) || sqlInjectionPattern.test(password)) {
                console.log(`\x1b[0m${req.ip} \x1b[33mLOGIN_REQUEST \x1b[0mfor ` + username + ` \x1b[0mmay be a SQL-Injection attempt! - \x1b[31m` + username + ' / ' + password);
                //trigger webhook
            }

            database.getConnection((err, connection) => {
                if (err) {
                    console.error('Error getting connection:', err);
                    return res.status(500).send(aesEncrypt(JSON.stringify({ error: 'connection' })));
                }

                const query = 'SELECT * FROM ?? WHERE LOWER(username) = LOWER(?) LIMIT 1';
                connection.query(query, [TABLE, username], (err, results) => {
                    connection.release();

                    if (err) {
                        console.error('Error querying database:', err);
                        return res.status(500).send(aesEncrypt(JSON.stringify({ error: 'connection' })));
                    }

                    if (results.length > 0) {
                        const result = results[0];

                        const hardwareIds = JSON.parse(result.hardwareIds);
                        if (!hardwareIds.includes(hwid) && !hardwareIds.includes('*')) {
                            console.log(`\x1b[0m${req.ip} \x1b[33mLOGIN_REQUEST \x1b[0mfor ` + username + ` \x1b[0mwas made from unregistered hardware identifier - \x1b[31m` + hwid);
                            //trigger webhook
                            return res.status(403).send(aesEncrypt(JSON.stringify({ error: 'rejected' })));
                        }

                        var restrictions = JSON.parse(result.restrictions);
                        if (restrictions.IP_ADDRESS && restrictions.IP_ADDRESS != req.ip) {
                            console.log(`\x1b[0m${req.ip} \x1b[33mLOGIN_REQUEST \x1b[0mfor ` + username + ` is ip address restricted`);
                            return res.status(403).send(aesEncrypt(JSON.stringify({ error: 'rejected' })));
                        }

                        if (restrictions.DISCORD_ID && restrictions.DISCORD_ID != discord) {
                            console.log(`\x1b[0m${req.ip} \x1b[33mLOGIN_REQUEST \x1b[0mfor ` + username + ` is discord id restricted - \x1b[31m` + discord);
                            if (!discord) {
                                return res.status(403).send(aesEncrypt(JSON.stringify({ error: 'rejected' })));
                            }

                            return res.status(403).send(aesEncrypt(JSON.stringify({ error: 'rejected' })));
                        }

                        if (result.licenseSuspended) {
                            console.log(`\x1b[0m${req.ip} \x1b[33mLOGIN_REQUEST \x1b[0mfor ` + username + ` \x1b[0mhas \x1b[31msuspended \x1b[0mlicense`);
                            return res.status(403).send(aesEncrypt(JSON.stringify({ error: 'suspended' })));
                        }

                        if (result.licenseExpiration < Date.now()) {
                            return res.status(403).send(aesEncrypt(JSON.stringify({ error: 'expired' })));
                        }

                        if (bcryptCompare(password, result.password)) {
                            return res.status(200).send(aesEncrypt(JSON.stringify({ token: result.licensekey, expiration: result.licenseExpiration, restrictions })));
                        }

                        return res.status(403).send(aesEncrypt(JSON.stringify({ error: 'rejected' })));
                    }

                    return res.status(404).send(aesEncrypt(JSON.stringify({ error: 'not_found' })));
                });
            });

            return;

        case "keep-alive":
            break;

        default:

            break;
    }

    //console.log(req.query);
    res.status(200).send('API v1');
});

module.exports = router;