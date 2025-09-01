const XOR_STRING_HASH = process.env.XOR_STRING_HASH.startsWith('0x')
    ? parseInt(process.env.XOR_STRING_HASH, 16)
    : parseInt(process.env.XOR_STRING_HASH, 10);

function generateXor(text) {
    let result = '';
    for (let i = 0; i < text.length; i++) {
        result += String.fromCharCode(text.charCodeAt(i) ^ XOR_STRING_HASH);
    }
    return result;
}

module.exports = { generateXor };