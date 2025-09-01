var express = require('express');
var router = express.Router();

router.get('/', function (req, res, next) {
    res.status(200).send('respond with a resource');
});

module.exports = router;
