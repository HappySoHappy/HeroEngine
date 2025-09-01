const crypto = require('crypto');
const { generateXor } = require('./xor');

const AES_STRING_HASH = process.env.AES_STRING_HASH;
const AES_KEY_SIZE = 32; // 32 bytes for AES-256
const AES_IV_SIZE = 16;  // 16 bytes for AES IV

function generateKey(size) {
    const hash = crypto.createHash('sha256').update(AES_STRING_HASH).digest();
    return hash.slice(0, size);
}

function aesEncrypt(text) {
    text = generateXor(text);
    const key = generateKey(AES_KEY_SIZE);
    const iv = generateKey(AES_IV_SIZE);

    const cipher = crypto.createCipheriv('aes-256-cbc', key, iv);
    let encrypted = cipher.update(text, 'utf8', 'base64');
    encrypted += cipher.final('base64');
    return encrypted;
}

function aesDecrypt(encryptedText) {
    const key = generateKey(AES_KEY_SIZE);
    const iv = generateKey(AES_IV_SIZE);

    const decipher = crypto.createDecipheriv('aes-256-cbc', key, iv);
    let decrypted = decipher.update(encryptedText, 'base64', 'utf8');
    decrypted += decipher.final('utf8');
    return generateXor(decrypted);
}

module.exports = { aesEncrypt, aesDecrypt };
