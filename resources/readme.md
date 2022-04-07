dbarone web site (.NET core)
============================

- [dbarone web site (.NET core)](#dbarone-web-site-net-core)
  - [linux host](#linux-host)
  - [database](#database)
  - [api](#api)
  - [ui](#ui)


linux host
----------

database
--------

Create:
- https://docs.microsoft.com/en-us/sql/linux/quickstart-install-connect-docker?view=sql-server-ver15&pivots=cs1-bash
- sudo docker pull mcr.microsoft.com/mssql/server:2019-latest
- sudo docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=<password>" -p 1433:1433 --name dbarone-sql1 --hostname dbarone-sql1 -d mcr.microsoft.com/mssql/server:2019-latest

api
---
- CLI: dotnet new webapi -n dbarone-api
- Storing database connection passwords: https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-6.0&tabs=windows
- 

ui
--
- Library: Preact https://preactjs.com/
- Bootstrap: `npx preact-cli create default dbarone-ui`
- File naming guide: https://github.com/airbnb/javascript/tree/master/react