p.application.sendActionRequest("resetUserPassword",Ld.fromData({email:a}),m(this,this.handleRequestResponse))};

p.application.sendActionRequest("autoLoginUser",Ld.fromData({existing_session_id:c,existing_user_id:d,client_id:this.get_uniqueClientIdValue(),
app_version:gs.get_clientVersion()}),m(this,this.handleRequestResponse))

Wa.prototype.sendActionRequest=function(a,b,c,d,e)




# Hero Zero  

## 📚 Table of Contents  
1. [Overview](#overview-herozero)  
2. [Architecture Overview](#architecture-overview)  

---

## 🌟 Architecture Overview  

### 🔒 Security  
- **Cloudflare** provides DDoS protection on `https://<SERVER>.herozerogame.com/` and all its sub-routes. Blocking typically occurs only under heavy load.  
- **Hashes** are used for all action requests made to `https://<SERVER>.herozerogame.com/request.php` under `auth` parameter is calculated with `MD5(<ACTION>GN1al351<USER_ID>)` where `GN1al351` appears to be constant salt value

### 🎯 Points of Interest  
- **CDN** for static data (icons, backgrounds, game values):  
  `https://hz-static-2.akamaized.net/`  
- **Client JavaScript**:  
  `https://hz-static-2.akamaized.net/assets/html5/HeroZero.min.js`  

### 🌐 API Endpoints  
- **Primary Communication**:  
  `https://<SERVER>.herozerogame.com/request.php` — Handles non-socket type communication between server and client.
- **Primary Socket Communication**:  
  `https://eu1a-sock1.herozerogame.com` — Handles socket type communication between server and client.
- **Live Game Status**:  
  `https://<SERVER>.herozerogame.com/infoMessage.php` — Provides live updates on game status (e.g., maintenance hours).

### Request Breakdown
- **Required Query Parameters**
  - action — Specifies what action to take, for example `loginUser`, `syncGame` or `claimDailyLoginBonus`
  - user_id — Numeric identifier for current user for example `1337`
  - user_session_id — String provided by server in loginUser response
  - client_version — Current version of game for browser prefixed with `html5_`
  - auth — Hash of request, calculated in following way: `MD5(<ACTION>GN1al351<USER_ID_>)`
  - build_number — Current build of the game
  - rct — Numberic value for Connection type (1 for HTTP or 2 for SOCKET)
  - keep_active — boolean, appears to be always true
  - device_type — Tied to `rct` parameter, (`web` for rct = 1 or `socket` for rct = 2)

- **Required Headers**
  - user-agent
  - accept
  - accept-encoding
  - accept-language
  - priority 
  - connection 
  - te