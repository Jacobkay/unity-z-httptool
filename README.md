# HttpTool

#### 介绍
简单好用的Unity http WebRequest请求，可配置token，配置返回值格式

#### 软件架构
软件架构说明


#### 安装教程

1.  下载程序，将文件夹拖入到工程中
2.  添加ZTools命名空间

#### 使用说明

1.  首先在DataRequest.cs脚本中设置好自己需要的返回值公共的外部数据格式类型，主要的数据类型以泛型实现
2.  设置Token：`HttpTool.Instance.Token = "123"; `
2.  设置超时时间：`HttpTool.Instance.TimeOut = 10;`
3.  Get请求：`HttpTool.Instance.Get<请求的数据格式>("url", data =>
            {
                Debug.Log("获得的数据列表：" + data.data);
            }, 是否需要token);`

4.  Post请求：`HttpTool.Instance.Post<请求的数据格式>("url",
            new 
            { 
                参数
            }    
            ,data =>
            {
                Debug.Log("获得的数据列表：" + data.data);
            }, 是否需要token);`

5.  Put请求：`HttpTool.Instance.Put<请求的数据格式>("url",
            new 
            { 
                参数
            }    
            ,data =>
            {
                Debug.Log("获得的数据列表：" + data.data);
            }, 是否需要token);`

6.  下载资源（文件未建立）：`HttpTool.Instance.DownloadFile<请求的数据格式>("url","下载路径","文件名", float loadnum =>
            {
                Debug.Log("下载进度：" + loadnum);
            }, 是否需要token);`

7.  下载资源（文件已建立）：`HttpTool.Instance.DownloadFile<请求的数据格式>("url","文件下载路径", float loadnum =>
            {
                Debug.Log("下载进度：" + loadnum);
            }, 是否需要token);`


#### 参与贡献

1.  Fork 本仓库
2.  新建 Feat_xxx 分支
3.  提交代码
4.  新建 Pull Request


#### 特技

1.  使用 Readme\_XXX.md 来支持不同的语言，例如 Readme\_en.md, Readme\_zh.md
2.  Gitee 官方博客 [blog.gitee.com](https://blog.gitee.com)
3.  你可以 [https://gitee.com/explore](https://gitee.com/explore) 这个地址来了解 Gitee 上的优秀开源项目
4.  [GVP](https://gitee.com/gvp) 全称是 Gitee 最有价值开源项目，是综合评定出的优秀开源项目
5.  Gitee 官方提供的使用手册 [https://gitee.com/help](https://gitee.com/help)
6.  Gitee 封面人物是一档用来展示 Gitee 会员风采的栏目 [https://gitee.com/gitee-stars/](https://gitee.com/gitee-stars/)
