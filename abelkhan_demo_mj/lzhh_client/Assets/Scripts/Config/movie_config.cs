/*this caller file is codegen by meter for c#*/
using System;
using System.Collections.Generic;

namespace meter
{
   public class movie_config
   {
       public float prelude;
       public float middle;
       public float ending;
       public string sayName;
       public string prefabName;
       public string movieSound;

       public movie_config( float _prelude, float _middle, float _ending, string _sayName, string _prefabName, string _movieSound )
       {
           prelude = _prelude;
           middle = _middle;
           ending = _ending;
           sayName = _sayName;
           prefabName = _prefabName;
           movieSound = _movieSound;
       }
   }

   public class movie_configs
   {
       public List<movie_config> tables;

       public movie_configs()
       {
           tables = new List<movie_config>{
               new movie_config( (float)0.0, (float)1.5, (float)0.0, "kuaidian", "emoji_chui", "knockTable" ),
               new movie_config( (float)0.0, (float)1.5, (float)0.0, "duoxie", "emoji_woshou", "" ),
               new movie_config( (float)0.0, (float)1.5, (float)0.0, "tiaoshui", "emoji_dianzan", "" ),
               new movie_config( (float)0.0, (float)1.5, (float)0.0, "guikuiban", "emoji_guokui", "eat" ),
               new movie_config( (float)0.0, (float)1.5, (float)0.0, "seeyou", "emoji_hua", "" ),
               new movie_config( (float)0.0, (float)2.5, (float)0.0, "jutai", "emoji_jutai", "leak" ),
               new movie_config( (float)0.0, (float)1.5, (float)0.0, "huimian", "emoji_mianfen", "blow" )
           };
       }

       static private movie_configs instance;
       static public movie_configs GetInstance()
       {
           if (instance == null)
           {
               instance = new movie_configs();
           }
           return instance;
       }
   }
}
