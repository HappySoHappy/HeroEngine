var express = require('express');
var router = express.Router();

router.get('/', function (req, res, next) {
    //throw new TypeError("Nuuh not allowed");
    res.status(200).send('respond with a resource');
});

module.exports = router;
