import { __private, Asset, assetManager, AssetManager, ImageAsset, isValid } from 'cc';

export class BundleManager
{
    private res:string="script/bundle/bundleManager.ts";
    private bundles:Map<string, AssetManager.Bundle> = new Map();
    public _battleBundlesReleased: boolean = true;

    public static _instance:BundleManager=null;
    static get Instance()
    {
        if(this._instance==null)
            this._instance=new BundleManager();
        return this._instance;
    }
    
    public loadBundle(bundleRes:string) : Promise<AssetManager.Bundle> {
        return new Promise((resolve) => {
            try {
                if(this.bundles.has(bundleRes))
                {
                    let bundle = this.bundles.get(bundleRes);
                    resolve(bundle);
                    return;
                }

                assetManager.loadBundle(bundleRes,(error,bundle) => {
                    if(error) {
                        console.warn(error.message);
                        resolve(null);
                    }
                    else {
                        this.bundles.set(bundleRes, bundle);
                        resolve(bundle);
                    }
                });
            }
            catch (err) {
                console.warn(this.res+"下的 loadAssets 错误:"+err);
                resolve(null);
            }    
        });
    }

    loadAssetsFromBundleSync(type: __private.__types_globals__Constructor<Asset> ,bundleRes:string, assetsRes:string ,_callBack:(data)=>void)
    {
        try
        {
            let bundle : AssetManager.Bundle = null;
            if(this.bundles.has(bundleRes))
            {
                bundle = this.bundles.get(bundleRes);
                bundle.load(assetsRes, type, (error, data) =>
                {
                    if (error)
                    {
                        console.warn("loadAssetsFromBundleSync 读取资源失败 :  ", error.message);
                        _callBack(null);
                    }
                    else
                    {
                        _callBack(data);
                    }
                });
            }
            else
            {
                assetManager.loadBundle(bundleRes,(error,bundle)=>
                {
                    if(error)
                    {
                        console.warn("loadAssetsFromBundleSync 读取bundle失败 :  ", error.message);
                        _callBack(null);
                    }
                    else
                    {
                        bundle.load(assetsRes, type ,(error,data)=>
                        {
                            if(error)
                            {
                                console.warn("loadAssetsFromBundleSync 读取资源失败 :  ", error.message);
                                _callBack(null);
                            }
                            else
                            {
                                _callBack(data);
                            }
                        });
                    }
                });
            }
        }
        catch(error)
        {
            console.error(this.res+"下的 loadAssetsFromBundleSync 错误:"+error);
            return null;
        }
        
    }

    loadAssetsFromBundle(bundleRes:string, assetsRes:string) : Promise<Asset> {   
        return new Promise(async (resolve, reject) => {
            const t0 = Date.now();
            try {
                let bundle = await this.loadBundle(bundleRes);
                const t1 = Date.now();
                
                bundle.load(assetsRes, Asset, (error, asset) => {
                    const t2 = Date.now();
                    console.log(`loadAssetsFromBundle "${bundleRes}/${assetsRes}": bundle=${t1-t0}ms, load=${t2-t1}ms`);
                    if(error) {
                        console.warn(`loadAssets '${bundleRes}' '${assetsRes}' error:`, error.message);
                        reject(error);
                    }
                    else {
                        resolve(asset);
                    }
                });
            }
            catch (err) {
                console.error(this.res+"下的 loadAssetsFromBundle 错误:"+err);
                reject(err);
            }    
        });
    }

    LoadImgsFromBundle(bundleRes:string, assetsRes:string): Promise<ImageAsset>
    {
        return new Promise(async (resolve) =>
        {
            try {
                let bundle = await this.loadBundle(bundleRes);

                bundle.load(assetsRes, ImageAsset, (error, img) => {
                    if(error) {
                        console.warn(this.res+"下的 LoadImgsFromBundle 没有读取到资源,err:", error.message);
                        resolve(null);
                    }
                    else {
                        resolve(img);
                    }
                });
            }
            catch (err) {
                console.warn(this.res+"下的 LoadImgsFromBundle 错误:"+err);
                resolve(null);
            }    
        });
    }
    

    loadAssetsFromUrl(url:string) : Promise<Asset> {
        return new Promise((resolve) => {
            try {
                assetManager.loadRemote(url, {ext:'.png'}, (err:Error, asset:Asset) => {
                    if (err) {
                        console.log(err.message);
                    }
                    resolve(asset);
                });
            }
            catch (err) {
                console.warn(this.res+"下的 loadAssets 错误:"+err);
                resolve(null);
            }    
        });
    }

    /**
     * 预加载bundle
     * @param _callBack 回调
     */
    public PreloadMustBundle(isPreload: boolean)
    {
        try
        {
            // 注意：SceneSpine、PanelSpine、ButtonTexture、BackGroungTexture、OtherTexture 已在 preloadMasterBundle 中加载
            //this.PreLoadBundleDir("PanelPrefabs", "", isPreload);
            this.PreLoadBundleDir("Sound", "", isPreload);
            //this.PreLoadBundleDir("Typeface","", isPreload);
            //this.PreLoadBundleDir("BoardPrefabs","", isPreload);
            //this.PreLoadBundleDir("RolesTexture","", isPreload);
        }
        catch (error)
        {
            console.warn(this.res + "下的 Preloading 错误:" + error);
        }
    }

    public PreloadWaitBundle(isPreload: boolean)
    {
        try
        {
            // 仅加载主界面及准备阶段常用资源
            // Battle、BulletPrefabs、PropsTexture 改为按需加载
            //this.PreLoadBundleDir("PanelTexture","", isPreload);
            this.PreLoadBundleDir("IconPrefabs","", isPreload);
            //this.PreLoadBundleDir("PartPrefabs","", isPreload);
            this.PreLoadBundleDir("TextTipPrefabs","", isPreload);
            this.PreLoadBundleDir("Quest","", isPreload);
            //this.PreLoadBundleDir("EffectSpine", "", isPreload);
        }
        catch (error)
        {
            console.warn(this.res + "下的 Preloading 错误:" + error);
        }
    }


    public NeedLoadBundle() : boolean {
        return this._battleBundlesReleased;
    }

    public async WaitLoadBundle() {
        await this.PreloadWaitBundle(false);
    }

    public async WaitLoadMust() {
        await this.PreloadMustBundle(false);
    }

    /**
     * 释放指定 bundle 的所有资源，场景切换或内存压力时调用
     * @param bundleName bundle名称
     */
    public ReleaseBundle(bundleName: string)
    {
        const bundle = this.bundles.get(bundleName);
        if (bundle)
        {
            bundle.releaseAll();
            this.bundles.delete(bundleName);
            assetManager.removeBundle(bundle);
            console.log(`已释放 bundle: ${bundleName}`);
        }
    }

    /**
     * 释放战斗相关 bundle，战斗结束后调用
     */
    public ReleaseBattleBundles()
    {
        this.ReleaseBundle("EffectSpine");
        this.ReleaseBundle("RoleSpine");
        this.ReleaseBundle("BulletPrefabs");
        this.ReleaseBundle("Battle");
        this._battleBundlesReleased = true;
    }

    /**
     * 预加载文件夹下的所有文件
     * @param _bundle 包名
     * @param _res 文件夹路径 （根目录填""）
     * @param _callBack 进度回调
     * @param _complete 完成回调
     * @param _timeout 超时时间（毫秒），默认30秒
     */
    async PreLoadBundleDir(_bundle:string, _res:string, _preload:boolean, _callBack?:((bundleName:string,progress:number)=>void)|null, _complete?:(()=>void)|null)
    {
        return new Promise<void>(async (resolve, reject) =>
        {
            try {
                let bundle = await this.loadBundle(_bundle);
                let info = bundle.getDirWithPath(_res);
                if (info)
                {
                    let n = 0;
                    for (let t of info)
                    {
                        let uuid = t.uuid;
                        let cachedAsset = assetManager.assets.get(uuid)
                        if (cachedAsset && isValid(cachedAsset))
                        {
                            n++;
                        }
                    }
                    if (n == info.length)
                    {
                        if (_callBack)
                        {
                            _callBack(_bundle, 100);
                        }
                        if (_complete)
                        {
                            _complete();
                        }
                        resolve();
                        return;
                    }
                }

                let onProgress = (finished: number, total: number, item: AssetManager.RequestItem) => {
                    if (_callBack)
                    {
                        _callBack(_bundle, Math.floor(finished / total * 100));
                    }
                };
                let onComplete = (err: Error | null, assets: AssetManager.RequestItem[]|Asset[]) => {
                    if (err)
                    {
                        console.error("加载失败:", _bundle, "/", _res, "错误:", err);
                        resolve();
                    }
                    else
                    {
                        console.log(`加载完成: ${_bundle}/${_res}, 共${assets.length}个资源)`);
                        if(_complete)
                        {
                            _complete();
                        }
                        resolve();
                    }
                }

                if (_preload) {
                    bundle.preloadDir(_res, onProgress, onComplete);
                }
                else {
                    bundle.loadDir(_res, onProgress, onComplete);
                }
            } catch (error) {
                console.error("PreLoadBundleDir 异常:", _bundle, "/", _res, "错误:", error);
                resolve();
            }
        })
    }
}


