#import <Foundation/Foundation.h>
#include "MXWechatConfig.h"

@interface WXApiManager : NSObject<WXApiDelegate>

+ (instancetype)sharedManager;

@end
