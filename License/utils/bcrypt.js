const bcrypt = require('bcrypt');

const BCRYPT_SALT_ROUNDS = 10;

function bcryptEncrypt(data) {
    try {
        const salt = bcrypt.genSaltSync(BCRYPT_SALT_ROUNDS);
        const hashedPassword = bcrypt.hashSync(data, salt);
        return hashedPassword;
    } catch (err) {
        throw err;
    }
}

function bcryptCompare(data, hashed) {
    try {
        const match = bcrypt.compareSync(data, hashed);
        return match;
    } catch (err) {
        console.error('Error comparing password:', err);
        return false;
    }
}

module.exports = { bcryptEncrypt, bcryptCompare };