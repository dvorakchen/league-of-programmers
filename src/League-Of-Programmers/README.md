## used status code
* 200
* 201 when create success
* 202 when create fault
* 400
* 404 resource not exist, for example, if a user not exist and client want to request it.
* 429

## Middleware
all middleware in directory Middlewares

* Antiforgery:		当客户端请求首页时往 cookie 里传递一个防伪令牌
* CircuitBreaker:	熔断

## Filters
all filters in directory Filters

* DisableFormValueModelBindingAttribute:		禁止模型绑定，流式上传大文件的接口使用
* ExceptionHandleAttribute:						全局异常处理
* FillDefaultResultInResponseIfNullAttribute:	当相应结果为 null 或空白字符时, 填充一个空对象
* GenerateAntiforgeryTokenCookieAttribute:		生成一个防伪 token 到 cookie 里