#import <Foundation/Foundation.h>

@interface MXWechatPayHandler : NSObject

+ (void)jumpToWxPay;
+ (void)payByPrepayID:(NSString *)prepayid;

@end
