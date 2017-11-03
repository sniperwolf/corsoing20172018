# CS Final Course Project

## [Service API] Local Development

### Apache

In `C:\xampp\apache\conf\httpd.conf`:

```
  LoadModule proxy_connect_module modules/mod_proxy_connect.so
  LoadModule proxy_http_module modules/mod_proxy_http.so
```

In `C:\xampp\apache\conf\extra\httpd-vhosts.conf`:

```
<VirtualHost *:80>
  ProxyPreserveHost On

  # Servers to proxy the connection, or;
  # List of application servers:
  # Usage:
  # ProxyPass / http://[IP Addr.]:[port]/
  # ProxyPassReverse / http://[IP Addr.]:[port]/
  # Example:
  ProxyPass /api/ http://localhost:5000/api/
  ProxyPassReverse /api http://localhost:5000/api/

  ServerName localhost
</VirtualHost>
```

### API

- **GET** `/api/students` (Get all Students)
- **PUT** `/api/students` (Add new Student) [*](#object)
- **PATCH** `/api/students/` (Update a Student) [*](#object)
- **DELETE** `/api/student/{id}`

<a name="object"></a> POSTMAN: Use `raw` => `json` and pass JSON student attributes (Case Sensitive):

```json
{
  "Name": "SSS"
}
```


___

Fabrizio Fallico (O55000331)