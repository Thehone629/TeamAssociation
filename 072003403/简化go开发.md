**七牛云**

```
https://goproxy.cn
 go env -w GO111MODULE=on
 go env -w GOPROXY=https://goproxy.cn,direct
```

阿里云

```
https://mirrors.aliyun.com/goproxy/
```

下载并安装gin

```
go get -u github.com/gin-gonic/gin
```

将 gin 引入到代码中

```
import "github.com/gin-gonic/gin"
```

（可选）如果使用诸如 `http.StatusOK` 之类的常量，则需要引入 `net/http` 包

```
import "net/http"
```

### 解决跨域问题

```
go get -u  github.com/gin-contrib/cors
```

下载安装gorm

```
go get -u gorm.io/gorm
go get -u gorm.io/driver/sqlite
```

或者(旧的方法)

```
go get -u github.com/jinzhu/gorm
```

引入驱动和gorm框架

```
import (
  "gorm.io/driver/mysql"
  "gorm.io/gorm"
)
```

自定义引入驱动

```
import (
  _ "example.com/my_mysql_driver"
  "gorm.io/driver/mysql"
  "gorm.io/gorm"
)
```



连接数据库

```
dsn := "user:pass@tcp(127.0.0.1:3306)/dbname?charset=utf8mb4&parseTime=True&loc=Local"
db, err := gorm.Open(mysql.Open(dsn), &gorm.Config{})
```

jwt轮询令牌

```
 go get github.com/dgrijalva/jwt-go
```

yml配置文件使用的第三方库

```
go get github.com/spf13/viper
```

### 接收各种参数的方法

接收单个参数

```
c.Param()
c.Query
c.DefaultQuery
c.PostForm
c.DefaultPostForm
c.QueryMap
c.PostFormMap
c.FormFile
c.MultipartForm
```

数据绑定

https://zhuanlan.zhihu.com/p/410246502

```
c.Bind
c.BindJSON
c.BindXML
c.BindQuery
c.BindYAML
c.ShouldBind
c.ShouldBindJSON
c.ShouldBindXML
c.ShouldBindQuery
c.ShouldBindYAML
```

### 密码加密

```
	//新建用户   加密算法（go中自带有这个函数库）
hasePassword, err := bcrypt.GenerateFromPassword([]byte(password), bcrypt.DefaultCost)
	if err != nil {
		response.Response(ctx, http.StatusUnprocessableEntity, 422, nil, "加密错误")
		//ctx.JSON(http.StatusUnprocessableEntity, gin.H{
		//	"msg": "加密错误",
		//})
		return
	}
	//判断密码是否正确
	if err := bcrypt.CompareHashAndPassword([]byte(account.Password), []byte(password)); err != nil {
		response.Response(ctx, http.StatusUnprocessableEntity, 422, nil, "密码错误")
		//ctx.JSON(http.StatusUnprocessableEntity, gin.H{
		//	"msg": "密码错误",
		//})
	}
```

### jwt token  

```
 go get github.com/dgrijalva/jwt-go
```

```
// jwt 加密密钥
var jwtKey = []byte("a_secret_creat")
//userId 和标准claims组成新的结构体
type Claims struct {
	UserId uint
	jwt.StandardClaims
}
//释放token
func ReleaseToken(user model.User) (string, error) {
	//设置token的有效期为七天
	expirationTime := time.Now().Add(7 * 24 * time.Hour)
	claims := &Claims{//claims才是真正要传送的数据
		UserId: user.ID,
		StandardClaims: jwt.StandardClaims{
			ExpiresAt: expirationTime.Unix(),
			IssuedAt:  time.Now().Unix(), //token发放时间
			Issuer:    "hss",
			Subject:   "user token", //主题
		},
	}
	token := jwt.NewWithClaims(jwt.SigningMethodHS256, claims)//将claims封装成一个token
	tokenString, err := token.SignedString(jwtKey)
	if err != nil {
		return "", err
	}
	//目标就是生成这个token字符串
	return tokenString, nil
}
//解析token获得claims
func ParseToken(tokenString string) (*jwt.Token, *Claims, error) {
	claims := &Claims{}
	token, err := jwt.ParseWithClaims(tokenString, claims, func(token *jwt.Token) (interface{}, error) {
		return jwtKey, nil
	})
	return token, claims, err
}
```



### token中密码敏感字段处理

```
import "learnOcearn/model"
//将完整结构体的部分数据封装为结构体返回给用户（注意使用时机）
type UserDto struct {
	Name      string `json:"name"`
	Telephone string `json:"telephone"`
}

func ToUserDto(user model.User) UserDto {
	return UserDto{
		Name:      user.Name,
		Telephone: user.Telephone,
	}
}
```

### 中间件middleware(也是需要上下文传递数据)

```
func AuthMiddleware() gin.HandlerFunc {
	return func(ctx *gin.Context) {
		//获取authorization  header
		tokenString := ctx.GetHeader("Authorization")
		//validate  token  验证
		if tokenString == "" || !strings.HasPrefix(tokenString, "Bearer") {
			ctx.JSON(http.StatusUnauthorized, gin.H{
				"code": 401,
				"msg":  "权限不足",
			})
			ctx.Abort()
			return
		}
		//Bearer 占了六位，从第七位开始解析，索引是六
		tokenString = tokenString[7:]
		token, claims, err := database.ParseToken(tokenString)
		if err != nil || !token.Valid {
			ctx.JSON(http.StatusUnauthorized, gin.H{
				"code": 401,
				"msg":  "权限不足",
			})
		}
		//通过验证后获取claim中的userId
		userId := claims.UserId
		DB := database.GetDB()
		var user model.User
		DB.First(&user, userId)
		//如果用户不存在说明这个token无效
		if user.ID == 0 {
			ctx.JSON(http.StatusUnauthorized, gin.H{
				"code": 401,
				"msg":  "权限不足",
			})
			ctx.Abort()
			return
		}
		//用户存在将user信息写入上下文
		ctx.Set("user", user)
		ctx.Next()
	}
}
```

### 封装统一的请求返回格式

```
import "learnOcearn/model"
type UserDto struct {
	Name      string `json:"name"`
	Telephone string `json:"telephone"`
}

func ToUserDto(user model.User) UserDto {
	return UserDto{
		Name:      user.Name,
		Telephone: user.Telephone,
	}
}

```



### 在项目中引入config文件配置

这里用的不是ini配置文件，而是yml配置文件

解析方法不同

install

```
go get github.com/spf13/viper
```

**application.yml基本配置**(还是用yml 因为Java也基本用这个)

```
server:
  port:
datasource:
  host: 127.0.0.1
  port: 3306
  database: ocean
  username: root
  password: 123456
```

```
func InitConfig() {
	workDir, _ := os.Getwd()
	//设置要读取的文件名
	viper.SetConfigName("application")
	//设置要读取的文件的类型
	viper.SetConfigType("yml")
	//设置读取文件的路径
	viper.AddConfigPath(workDir + "/config")
	err := viper.ReadInConfig()
	if err != nil {
		panic(err)
	}
}
```



### database连接数据库基本模板

```
package database

import (
	"fmt"
	"github.com/spf13/viper"
	"gorm.io/driver/mysql"
	"gorm.io/gorm"
	"learnOcearn/model"
)

var DB *gorm.DB
//因为是在yml中的配置，然后viper进行的解析
func InitDB() {
	userName := viper.GetString("datasource.username")
	password := viper.GetString("datasource.password")
	host := viper.GetString("datasource.host")
	port := viper.GetString("datasource.port")
	database := viper.GetString("datasource.database")
	args := fmt.Sprintf("%s:%s@tcp(%s:%s)/%s?charset=utf8mb4&parseTime=true",
		userName,
		password,
		host,
		port,
		database)
	db, err := gorm.Open(mysql.Open(args), &gorm.Config{})
	if err != nil {
		panic(err.Error)
	}
	DB = db
	_ = db.AutoMigrate(&model.User{})
}
func GetDB() *gorm.DB {
	return DB//上面Init在main中被调用，因此这个DB是赋值后的
}
```

### 随机算法获得一串字符串（封装在util工具包中）

如果作为用户名，严谨点还要掉数据库看是否唯一

```
func RandString(n int) string {
	var letters = []byte("abcdefghijklmnopqrstuvwxyz")
	result := make([]byte, n)
	for i := range result {
		result[i] = letters[rand.Intn(len(letters))]
	}
	return string(result)
}
```

### 一种非常好的风格

router中

```
func CollectRouter(r *gin.Engine) *gin.Engine {
	r.POST("/api/auth/register", controller.Register)
	r.POST("/api/auth/login", controller.Login)
	//中间件用来保护访问用户信息的接口
	r.GET("/api/auth/info", middleware.AuthMiddleware(), controller.Info)
	return r
}
```

main中

```
func main() {
	InitConfig()
	database.InitDB()

	r := gin.Default()
	//主要是下面这行代码   
	r = CollectRouter(r)
	_ = r.Run()
}
```





