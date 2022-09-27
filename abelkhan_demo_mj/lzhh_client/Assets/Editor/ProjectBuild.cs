using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
#if UNITY_IOS || UNITY_EDITOR_OSX
using UnityEditor.iOS.Xcode; //XcodeAPI
#endif
using UnityEditor.Callbacks;
using System.Collections;
using System.Collections.Generic;
using System.IO;
public class ProjectBuild {

	//<summary>
	//生成Xcode工程
	//</summary>
	static void BuildForIPhone()
	{
		Debug.Log ("BuildForIPhone");
		// BuildPipeline.BuildPlayer(GetBuildScenes(), Globals.ProjectName, BuildTarget.iOS, BuildOptions.None);
		BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
		buildPlayerOptions.scenes = GetBuildScenes();
        buildPlayerOptions.locationPathName = "iOSBuild";
		buildPlayerOptions.target = BuildTarget.iOS;
		buildPlayerOptions.options = BuildOptions.None;
		BuildPipeline.BuildPlayer (buildPlayerOptions);
	}

    //<summary>
    //生成安卓APK包
    //</summary>
    static void BuildAPK()
    {
        Debug.Log("BuildAPK");
        // BuildPipeline.BuildPlayer(GetBuildScenes(), Globals.ProjectName, BuildTarget.iOS, BuildOptions.None);
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = GetBuildScenes();
        buildPlayerOptions.locationPathName = "AndroidBuild";
        buildPlayerOptions.target = BuildTarget.Android;
        buildPlayerOptions.options = BuildOptions.None;
        BuildPipeline.BuildPlayer(buildPlayerOptions);
    }

    static string[] GetBuildScenes()
    {
        return new[] {
			"Assets/Scenes/UGUI_New/DontDestroyScene.unity",
            "Assets/Scenes/UGUI_New/NULogin.unity",
            "Assets/Scenes/UGUI_New/NUMainWindow.unity",
			"Assets/InGame.unity"
		};
    }

	#if UNITY_IOS || UNITY_EDITOR_OSX
	// ios版本xcode工程维护代码
	[PostProcessBuild]
	public static void OnPostprocessBuild(BuildTarget BuildTarget, string path)
	{

		Debug.Log("Build path:" + path);
		if (BuildTarget == BuildTarget.iOS)
		{
			Debug.Log("BuildTarget == BuildTarget.iOS");
			string projPath = PBXProject.GetPBXProjectPath(path);
			Debug.Log("PBXProject path:" + projPath);
			PBXProject proj = new PBXProject();
			proj.ReadFromString(File.ReadAllText(projPath));

			// 获取当前项目名字
			string target = proj.TargetGuidByName(PBXProject.GetUnityTargetName());
			Debug.Log("target path:" + target);
			// 对所有的编译配置设置选项
			proj.SetBuildProperty(target, "ENABLE_BITCODE", "NO");

			// 添加依赖库
			// weixin sdk
			//proj.AddFrameworkToProject (target, "SystemConfiguration.framework", false);
			proj.AddFrameworkToProject (target, "libz.tbd", false);
			proj.AddFrameworkToProject (target, "libsqlite3.0.tbd", false);
			proj.AddFrameworkToProject (target, "libc++.tbd", false);
			proj.AddFrameworkToProject (target, "Security.framework", false);
			//proj.AddFrameworkToProject (target, "CoreTelephony.framework", false);
			proj.AddFrameworkToProject (target, "CFNetwork.framework", false);
			proj.AddFrameworkToProject (target, "MobileCoreServices.framework", false);
			// GVoice sdk
			proj.AddFrameworkToProject (target, "SystemConfiguration.framework", false);
			proj.AddFrameworkToProject (target, "CoreTelephony.framework", false);
			proj.AddFrameworkToProject (target, "AudioToolbox.framework", false);
			proj.AddFrameworkToProject (target, "CoreAudio.framework", false);
			proj.AddFrameworkToProject (target, "AVFoundation.framework", false);
			proj.AddFrameworkToProject (target, "libstdc++.6.0.9.tbd", false);
			// UnityAppController.mm add -fno-objc-arc flag
			string file = proj.FindFileGuidByProjectPath("Classes/UnityAppController.mm");
			var flags = proj.GetCompileFlagsForFile(target, file);
			flags.Add("-fno-objc-arc");
			proj.SetCompileFlagsForFile(target, file, flags);

			proj.AddBuildProperty(
				target, "HEADER_SEARCH_PATHS", "$(SRCROOT)/Libraries/Plugins/iOS"
			);
			proj.AddBuildProperty(target, "OTHER_LDFLAGS", "-ObjC");
			proj.AddBuildProperty(target, "OTHER_LDFLAGS", "-force_load $(SRCROOT)/Libraries/Plugins/iOS/libWeChatSDK.a");
			proj.SetBuildProperty(target, "ENABLE_BITCODE", "NO");
			proj.SetBuildProperty(target, "GCC_ENABLE_OBJC_EXCEPTIONS", "YES");
			// 保存工程
			proj.WriteToFile (projPath);

			// 修改plist
			string plistPath = path + "/Info.plist";
			PlistDocument plist = new PlistDocument();
			plist.ReadFromString(File.ReadAllText(plistPath));
			PlistElementDict rootDict = plist.root;
			// weixin 相关
			{
				var arr = rootDict.CreateArray ("CFBundleURLTypes");
				{
					var dict = arr.AddDict ();
					dict.SetString ("CFBundleTypeRole", "Editor");
					dict.SetString ("CFBundleURLName", "weixin");
					var arr1 = dict.CreateArray ("CFBundleURLSchemes");
					arr1.AddString ("wxafd936830b3cf60f");
				}
			}
			{
				var arr = rootDict.CreateArray ("LSApplicationQueriesSchemes");
				arr.AddString ("wechat");
				arr.AddString ("weixin");
			}
			{
				var arr = rootDict.CreateArray ("UIBackgroundModes");
				arr.AddString ("remote-notification");
			}
			rootDict.SetString ("NSMicrophoneUsageDescription", "语音需要打开麦克风");
			// 保存plist
			plist.WriteToFile (plistPath);

			// UnityAppController.mm代码修改
			EditorCode(path);
		}
	}

	private static void EditorCode (string filePath)
	{
		UnityEditor.XCodeEditor.XClass UnityAppControllerH = new UnityEditor.XCodeEditor.XClass(filePath + "/Classes/UnityAppController.h");
		UnityAppControllerH.WriteBelow ("#include \"PluginBase/RenderPluginDelegate.h\"", "#include \"WXApi.h\"");
		UnityAppControllerH.Replace ("@interface UnityAppController : NSObject<UIApplicationDelegate>", "@interface UnityAppController : NSObject<UIApplicationDelegate, WXApiDelegate>");
		UnityAppControllerH.WriteBelow ("- (void)startUnity:(UIApplication*)application;", "+ (UIImage *)imageWithImage:(UIImage *)image scaledToSize:(CGFloat)newSize;");

        UnityEditor.XCodeEditor.XClass UnityAppControllerMM = new UnityEditor.XCodeEditor.XClass(filePath + "/Classes/UnityAppController.mm");
		UnityAppControllerMM.WriteBelow("#include \"PluginBase/AppDelegateListener.h\"", "#include \"WechatTool/MXWechatConfig.h\"");
		UnityAppControllerMM.WriteBelow("@implementation UnityAppController", "#pragma mark - WXApiDelegate\n\n#define WeiXinID @\"wxafd936830b3cf60f\"\n\n#define WeiXinSecret @\"e72afcd7738d3fb3c6a6600f7a8f299f\"\n\n#define ksendAuthRequestNotification @\"ksendAuthRequestNotification\"\n// 支付\n#define ksendPayRequestNotification @\"ksendPayRequestNotification\"\n\n#define GameObjectName \"Logic\"\n\n#define MethodName \"GetCode\"\n\n#define ShareMethod \"Weixincallback_shareSuccess\"\n\nextern \"C\"\n{\n    bool isWXAppInstalled()\n    {\n        return [WXApi isWXAppInstalled];\n    }\n    bool isWXAppSupportApi()\n    {\n        return [WXApi isWXAppSupportApi];\n    }\n    // 给Unity3d调用的方法\n    void weixinLoginByIos()\n    {\n        // 登录\n        [[NSNotificationCenter defaultCenter] postNotificationName:ksendAuthRequestNotification object:nil];\n    }\n    \n    void weixinPay()\n    {\n        //发起微信支付\n        [MXWechatPayHandler jumpToWxPay];\n\n        // 登录\n        // [[NSNotificationCenter defaultCenter] postNotificationName:ksendPayRequestNotification object:nil];\n    }\n    \n    void weixinPayByPrepayID(const char* prepayid)\n    {\n        //发起微信支付\n        NSString *prepayidstr=[NSString stringWithUTF8String:prepayid];\n        [MXWechatPayHandler payByPrepayID:prepayidstr];\n        \n        // 登录\n        // [[NSNotificationCenter defaultCenter] postNotificationName:ksendPayRequestNotification object:nil];\n    }\n\n    \n    void ShareByIos(const char* title,const char*desc,const char*url)\n    {\n        NSString *urlStr=[NSString stringWithUTF8String:url];\n        NSString *filePath = [[NSBundle mainBundle]pathForResource:urlStr ofType:@\"png\"];\n        WXImageObject *ext = [WXImageObject object];\n        UIImage *image = [UIImage imageNamed:urlStr];\n        NSData *data = UIImageJPEGRepresentation(image,0.5);\n        ext.imageData = data;\n        //ext.imageUrl = filePath ;\n        WXMediaMessage *message = [WXMediaMessage message];\n        UIImage *imagea = [UIImage imageWithData:data];\n        UIImage* thumbImage = [UnityAppController imageWithImage:image scaledToSize:320];\n        message.mediaObject = ext;\n        [message setThumbImage:thumbImage];\n        SendMessageToWXReq* req = [[[SendMessageToWXReq alloc] init]autorelease];\n        req.bText = NO;\n        req.message = message;\n        req.scene = WXSceneSession;\n        \n        [WXApi sendReq:req];\n    }\n    \n    void ShareWebByIos(const char* title,const char*desc,const char*url)\n    {\n        NSString *titleStr=[NSString stringWithUTF8String:title];\n        NSString *descStr=[NSString stringWithUTF8String:desc];//0416aa28b5d2ed1f3199083b3806c6bl\n        NSString *urlStr=[NSString stringWithUTF8String:url];\n        NSLog(@\"ShareByIos titleStr:%@\",titleStr);\n        NSLog(@\"ShareByIos descStr:%@\",descStr);\n        NSLog(@\"ShareByIos urlStr:%@\",urlStr);\n        //UIImage *img=[UIImage imageNamed:urlStr];\n        //                        NSLog(@\"ShareByIos img:%@\",img);\n        // 分享\n        WXMediaMessage *message = [WXMediaMessage message];\n        message.title = titleStr;\n        message.description = descStr;\n        [message setThumbImage:[UIImage imageNamed:@\"AppIcon72x72\"]];\n        //[message setThumbImage:[UIImage imageNamed:urlStr]];\n        \n        WXWebpageObject *ext = [WXWebpageObject object];\n        ext.webpageUrl = urlStr;\n        \n        message.mediaObject = ext;\n        message.mediaTagName = @\"WECHAT_TAG_SHARE\";\n        //message.thumbData = UIImagePNGRepresentation(img);\n        //[message setThumbImage:img];\n        \n        SendMessageToWXReq* req = [[[SendMessageToWXReq alloc] init]autorelease];\n        req.bText = NO;\n        req.message = message;\n        req.scene = WXSceneSession;\n        [WXApi sendReq:req];\n    }\n}\n\n- (BOOL)application:(UIApplication *)application handleOpenURL:(NSURL *)url\n{\n    return [WXApi handleOpenURL:url delegate:self];\n}\n\n- (void)onReq:(BaseReq *)req // 微信向第三方程序发起请求,要求第三方程序响应\n{\n    \n}\n\n- (void)onResp:(BaseResp *)resp // 第三方程序向微信发送了sendReq的请求,那么onResp会被回调\n{\n    if([resp isKindOfClass:[SendAuthResp class]]) // 登录授权\n    {\n        SendAuthResp *temp = (SendAuthResp*)resp;\n        if(temp.code!=nil)UnitySendMessage(GameObjectName, MethodName, [temp.code cStringUsingEncoding:NSUTF8StringEncoding]);\n        \n        //        [self getAccessToken:temp.code];\n    }\n    else if([resp isKindOfClass:[SendMessageToWXResp class]])\n    {\n        // 分享\n        if(resp.errCode==0)\n        {\n            NSString *code = [NSString stringWithFormat:@\"%d\",resp.errCode]; // 0是成功 -2是取消\n            NSLog(@\"SendMessageToWXResp:%@\",code);\n            UnitySendMessage(GameObjectName, ShareMethod, [code cStringUsingEncoding:NSUTF8StringEncoding]);\n        }\n    }\n    else if([resp isKindOfClass:[PayResp class]])\n    {\n        NSString *code = [NSString stringWithFormat:@\"%d\",resp.errCode]; // 0是成功 -2是取消\n        NSLog(@\"PayResp:%@\",code);\n        if(resp.errCode==0)\n        {\n            \n            UnitySendMessage(GameObjectName, \"PaySuccess\", [@\"1\" cStringUsingEncoding:NSUTF8StringEncoding]);\n        }\n        else if(resp.errCode==-2)\n        {\n            UnitySendMessage(GameObjectName, \"PaySuccess\", [@\"2\" cStringUsingEncoding:NSUTF8StringEncoding]);\n        }\n        else\n        {\n            UnitySendMessage(GameObjectName, \"PaySuccess\", [@\"3\" cStringUsingEncoding:NSUTF8StringEncoding]);\n        }\n    }\n}\n\n#pragma mark - Private\n\n- (void)sendAuthRequest\n\n{\n    SendAuthReq* req = [[[SendAuthReq alloc] init] autorelease];\n    req.scope = @\"snsapi_userinfo\";\n    req.state = @\"only123\";\n    [WXApi sendAuthReq:req viewController:_rootController delegate:self];\n    \n}\n\n- (void)sendPayRequest\n\n{\n    PayReq *req = [[[PayReq alloc] init] autorelease];\n    req.partnerId = @\"1449873902\";\n    req.prepayId= @\"1101000000140415649af9fc314aa427\";\n    req.package = @\"Sign=WXPay\";\n    NSTimeInterval time = [[NSDate date] timeIntervalSince1970];\n    req.nonceStr= @\"a462b76e7436e98e0fjhe13c64b4fd1c\";\n    req.timeStamp= 12345;\n    req.sign= @\"582282D72DD2B03AD892830965F428CB16E7A256\";\n    [WXApi sendReq:req];\n}\n\n\n\n- (void)getAccessToken:(NSString *)code\n\n{\n    \n    NSString *path = [NSString stringWithFormat:@\"https://api.weixin.qq.com/sns/oauth2/access_token?appid=%@&secret=%@&code=%@&grant_type=authorization_code\",WeiXinID,WeiXinSecret,code];\n    \n    NSURLRequest *request = [[NSURLRequest alloc] initWithURL:[NSURL URLWithString:path] cachePolicy:NSURLRequestUseProtocolCachePolicy timeoutInterval:10.0];\n    \n    NSOperationQueue *queue = [[NSOperationQueue alloc] init];\n    \n    [NSURLConnection sendAsynchronousRequest:request queue:queue completionHandler:\n     \n     ^(NSURLResponse *response,NSData *data,NSError *connectionError)\n     \n     {\n         \n         if (connectionError != NULL)\n             \n         {\n             \n         }\n         else\n             \n         {\n             \n             if (data != NULL)\n                 \n             {\n                 \n                 NSError *jsonParseError;\n                 \n                 NSDictionary *responseData = [NSJSONSerialization JSONObjectWithData:data options:NSJSONReadingMutableContainers error:&jsonParseError];\n                 \n                 NSLog(@\"#####responseData = %@\",responseData);\n                 \n                 if (jsonParseError != NULL)\n                     \n                 {\n                     \n                     //                    NSLog(@\"#####responseData = %@\",jsonParseError);\n                     \n                 }\n                 \n                 NSString *accessToken = [responseData valueForKey:@\"access_token\"];\n                 \n                 NSString *openid = [responseData valueForKey:@\"openid\"];\n                 \n                 [self getUserInfo:accessToken withOpenID:openid];\n                 \n             }\n             \n         }\n         \n     }];\n    \n}\n\n- (void)getUserInfo:(NSString *)accessToken withOpenID: (NSString *)openid\n\n{\n    \n    NSString *path = [NSString stringWithFormat:@\"https://api.weixin.qq.com/sns/userinfo?access_token=%@&openid=%@\",accessToken,openid];\n    \n    NSURLRequest *request = [[NSURLRequest alloc] initWithURL:[NSURL URLWithString:path] cachePolicy:NSURLRequestUseProtocolCachePolicy timeoutInterval:10.0];\n    \n    NSOperationQueue *queue = [[NSOperationQueue alloc] init];\n    \n    [NSURLConnection sendAsynchronousRequest:request queue:queue completionHandler:\n     \n     ^(NSURLResponse *response,NSData *data,NSError *connectionError) {\n         \n         if (connectionError != NULL) {\n             \n             \n             \n         } else {\n             \n             if (data != NULL) {\n                 \n                 NSError *jsonError;\n                 \n                 NSString *responseData = [NSJSONSerialization JSONObjectWithData:data options:NSJSONReadingMutableContainers error:&jsonError];\n                 \n                 NSLog(@\"#####responseData = %@\",responseData);\n                 \n                 NSString *jsonData = [NSString stringWithFormat:@\"%@\",responseData];\n                 \n                 UnitySendMessage(GameObjectName, MethodName, [jsonData cStringUsingEncoding:NSUTF8StringEncoding]);\n                 \n                 if (jsonError != NULL) {\n                     \n                     //                     NSLog(@\"#####responseData = %@\",jsonError);\n                     \n                 }\n                 \n             }\n             \n         }\n         \n     }];\n    \n}\n#pragma mark -\n");
		UnityAppControllerMM.Replace ("NSMutableArray* keys\t= [NSMutableArray arrayWithCapacity:3];\n\tNSMutableArray* values\t= [NSMutableArray arrayWithCapacity:3];\n\n\tauto addItem = [&](NSString* key, id value)\n\t{\n\t\t[keys addObject:key];\n\t\t[values addObject:value];\n\t};\n\n\taddItem(@\"url\", url);\n\taddItem(@\"sourceApplication\", sourceApplication);\n\taddItem(@\"annotation\", annotation);\n\n\tNSDictionary* notifData = [NSDictionary dictionaryWithObjects:values forKeys:keys];\n\tAppController_SendNotificationWithArg(kUnityOnOpenURL, notifData);\n\treturn YES;", "NSMutableArray* keys    = [NSMutableArray arrayWithCapacity:3];\n    NSMutableArray* values    = [NSMutableArray arrayWithCapacity:3];\n    \n#define ADD_ITEM(item)    do{ if(item) {[keys addObject:@#item]; [values addObject:item];} }while(0)\n    \n    ADD_ITEM(url);\n    ADD_ITEM(sourceApplication);\n    ADD_ITEM(annotation);\n    \n#undef ADD_ITEM\n    \n    NSDictionary* notifData = [NSDictionary dictionaryWithObjects:values forKeys:keys];\n    AppController_SendNotificationWithArg(kUnityOnOpenURL, notifData);\n\treturn [WXApi handleOpenURL:url delegate:self];");
		UnityAppControllerMM.WriteBelow("[KeyboardDelegate Initialize];\n", "// wx\n    [[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(sendAuthRequest) name:ksendAuthRequestNotification object:nil]; // 微信登录\n    [[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(sendPayRequest) name:ksendPayRequestNotification object:nil]; // 微信支付\n\n    \n    //向微信注册\n    [WXApi registerApp:WeiXinID];");
		UnityAppControllerMM.WriteBelow ("- (void)startUnity:(UIApplication*)application\n{\n\tNSAssert(_unityAppReady == NO, @\"[UnityAppController startUnity:] called after Unity has been initialized\");\n\n\tUnityInitApplicationGraphics(UNITY_FORCE_DIRECT_RENDERING);\n\n\t// we make sure that first level gets correct display list and orientation\n\t[[DisplayManager Instance] updateDisplayListInUnity];\n\n\tUnityLoadApplication();\n\tProfiler_InitProfiler();\n\n\t[self showGameUI];\n\t[self createDisplayLink];\n\n\tUnitySetPlayerFocus(1);\n}", "\n+ (UIImage *)imageWithImage:(UIImage *)image scaledToSize:(CGFloat)newSize\n{\n    CGFloat W = image.size.width;\n    CGFloat H = image.size.height;\n    CGFloat scaleFactorW = newSize / W;\n    CGFloat scaleFactorH = newSize / H;\n    CGFloat scaleFactor = MIN(scaleFactorH, scaleFactorW);\n    CGFloat newW = W * scaleFactor;\n    CGFloat newH = H * scaleFactor;\n    \n    if (W < newSize || H < newSize) {\n        return image;\n    }\n    \n    CGSize size = CGSizeMake(newW, newH);\n    UIGraphicsBeginImageContext(size);\n    [image drawInRect:CGRectMake(0, 0, size.width, size.height)];\n    UIImage *newImage = UIGraphicsGetImageFromCurrentImageContext();\n    UIGraphicsEndImageContext();\n    return newImage;\n}\n");
		
	}
	#endif
}
