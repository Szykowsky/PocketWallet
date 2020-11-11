# PocketWallet
Password Wallet - backend, ASP .NET CORE 3.1 + Unit tests

If you want to test without download repository make docker-compose.yml file and paste this code. This create an API container with database a get image from my repository on docker hub.

version: '3.8'
```javascript
services:
  Full2020-86424:
    container_name: Full2020-86424
    image: mcr.microsoft.com/mssql/server
    environment:
      SA_PASSWORD: 'Password@12345'
      ACCEPT_EULA: 'Y'
    ports:
      - 1433:1433
    networks:
      pocketwallet-bridge:
        ipv4_address: 10.0.10.3
  pocketwallet:
    image: szykowsky/pocket-wallet
    ports:
      - '9080:80'
    environment:
      - 'ConnectionStrings:PasswordWallet=Server=Full2020-86424,1433;Database=PasswordWallet;User=sa;Password=Password@12345;Trusted_Connection=False;'
    depends_on:
      - Full2020-86424
    networks:
      pocketwallet-bridge:
        ipv4_address: 10.0.10.4

networks:
  pocketwallet-bridge:
    driver: bridge
    ipam:
      config:
        - subnet: 10.0.10.0/24
 ```
