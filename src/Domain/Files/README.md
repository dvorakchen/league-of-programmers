## Files

文件类
在配置文件中配置相关参数
```
"File": {
  "AvatarFileSizeLimet": 64,    //  上传的头像最大字节，默认 64字节
  "FileSizeLimit": 5242880,     //  上传文件最大字节，默认 9M
  "SaveWebPath": "/files/",     //  文件保存路径，从根目录开始
  "SaveThumbnailWebPath": "/files/thumbnail/",  //  缩略图文件保存路径，从根目录开始
  "Template": "/files/temp",    //  临时文件文件保存路径，从根目录开始
  "AllowFileExtension": ".jpg,.png,.bmp,.jpeg,.gif,.doc,.docx,.xls,.ppt,.pptx", //  允许的文件格式
  "AllowAvatarExtension": ".jpg,.png,.bmp,.jpeg,.gif"               //  允许的头像文件格式
}
```

### 文件结构
* File              外部调用的话，通过这个类
* Validation        文件验证
* FileSigntures     文件签名验证

### 缩略图
保存图片的缩略图使用 File.GetThumbnail(filePath)
或将缩略图保存在配置文件指定的 SaveThumbnailWebPath 中