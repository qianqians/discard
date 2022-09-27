using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
public class TestExportSprite
{

    [MenuItem("Assets/导出选中图片为单独png")]
    static void ExportSelSprite()
    {
        string resourcesPath = "Assets/Resources/";
        foreach (Object obj in Selection.objects)
        {
            string selectionPath = AssetDatabase.GetAssetPath(obj);

            // 必须最上级是"Assets/Resources/"
            if (selectionPath.StartsWith(resourcesPath))
            {
                string selectionExt = System.IO.Path.GetExtension(selectionPath);
                if (selectionExt.Length == 0)
                {
                    continue;
                }

                // 得到导出路径
                string loadPath = selectionPath.Remove(selectionPath.Length - selectionExt.Length);
                loadPath = loadPath.Substring(resourcesPath.Length);

                // 加载此文件下的所有资源
                Sprite[] sprites = Resources.LoadAll<Sprite>(loadPath);
                if (sprites.Length >0)
                {
                    // 创建导出文件夹
                    string outPath = Application.dataPath + "/outSprite/" + loadPath;
                    System.IO.Directory.CreateDirectory(outPath);

       
                        //  for (int i = 0; i < 10; i++)
                    foreach (Sprite sprite in sprites)
                    {
                      //  Sprite sprite = sprites[i];
                        //string[] name = sprite.name.Split(new char[] { '_' });
                        //int inde = int.Parse(name[2]);
                        //if (inde>0)
                        //{
                        //    continue;
                        //}
                        // 创建单独的纹理
                        Texture2D tex = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height, sprite.texture.format, false);
                        //   tex.rgb
          
                        if (tex != null)
                        {
                            try
                            {
                              //  Color[] colorList = tex.GetPixels((int)sprite.rect.xMin, (int)sprite.rect.yMin, (int)sprite.rect.width, (int)sprite.rect.height);

                                //if (colorList != null)
                                //{

                                    tex.SetPixels(sprite.texture.GetPixels((int)sprite.rect.xMin, (int)sprite.rect.yMin,(int)sprite.rect.width, (int)sprite.rect.height));
                                    tex.Apply();
                                    // sprite.
                                    // 写入成PNG文件
                                    System.IO.File.WriteAllBytes(outPath + "/" + sprite.name + ".png", tex.EncodeToPNG());
                                //}
                              
                            }
                            catch (System.Exception e)
                            {

                            }
            
                        }
                    
                    }
                    Debug.Log(string.Format("Export {0} to {1}", loadPath, outPath));
                }
            }
        }
        Debug.Log("Export All Sprites Finished");
    }
}