var createError = require('http-errors');
var express = require('express');
var path = require('path');
var logger = require('morgan');

require('dotenv').config();

var apiRouter = require('./routes/api');
var panelRouter = require('./routes/panel');
var adminPanelRouter = require('./routes/adminPanel');
var usersRouter = require('./routes/users');

var app = express();

app.set('views', path.join(__dirname, 'views'));
app.set('view engine', 'pug');

logger.token('statusColor', (req, res, args) => {
    var status = (typeof res.headersSent !== 'boolean' ? Boolean(res.header) : res.headersSent)
        ? res.statusCode
        : undefined

    var color = status >= 500 ? 31 // red
        : status >= 400 ? 33 // yellow
            : status >= 300 ? 36 // cyan
                : status >= 200 ? 32 // green
                    : 0; // no color

    return '\x1b[' + color + 'm' + status + '\x1b[0m';
});
app.use(logger(`\x1b[0m:remote-addr \x1b[33m:method\x1b[0m :url\x1b[0m :statusColor :response-time ms - length=:res[content-length]`));

app.use(express.json());
app.use(express.urlencoded({ extended: false }));
app.use(express.text());

app.use(express.static(path.join(__dirname, 'public', 'static')));

app.use('/api', apiRouter);
app.use('/panel', panelRouter);
app.use('/admin', adminPanelRouter);
app.use('/users', usersRouter);

app.use(function (req, res, next) {
    next(createError(404));
});

app.use(function (err, req, res, next) {
    //req.socket.destroy();
    res.status(500).send();
    return;

    /*res.locals.message = err.message;
    res.locals.error = req.app.get('env') === 'development' ? err : {};
    res.status(err.status || 500);
    res.render('error');*/
});

/*const readline = require('readline');

const rl = readline.createInterface({
    input: process.stdin,
    output: process.stdout
});

rl.on('line', (input) => {
    console.log('Received command: ' + input);

    const args = input.split(' ');

    const command = args[0].toLowerCase();
    const user = args[1];

    switch (command) {
        case 'exit':
            console.log('Exiting...');
            rl.close();
            break;
        case 'status':
            console.log('Server is running...');
            break;
        case 'license':
            if (args[1] === 'revoke' && args[2]) {
                const userToRevoke = args[2];
                console.log(`Revoking license for user: ${userToRevoke}`);
            } else {
                console.log('Invalid command. Usage: license revoke <user>');
            }
            break;
        default:
            console.log('Unknown command');
            break;
    }
});*/

module.exports = app;
