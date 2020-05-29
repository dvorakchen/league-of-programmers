## Domain

主要的对象会放在这个类库下面

* Users		用户，包括管理员和客户都是用户

### Users
IUserManager:	此接口主要用户依赖注入
UserManager:	用户管理，实现 IUserManager
User:			用户的抽象类，管理员和客户都继承自这个抽象类
Client:			客户，继承自 User
Administrator:	管理员，继承自 User
Models:			API 接受参数模型
UserCache:		用户缓存