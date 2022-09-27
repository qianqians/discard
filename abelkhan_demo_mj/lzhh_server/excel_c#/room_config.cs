/*this caller file is codegen by meter for c#*/
using System;
using System.Collections.Generic;

namespace meter
{
   public class room_config
   {
       public int times;
       public int playerNum;
       public int payRule;
       public int pay;

       public room_config( int _times, int _playerNum, int _payRule, int _pay )
       {
           times = _times;
           playerNum = _playerNum;
           payRule = _payRule;
           pay = _pay;
       }
   }

   public class room_configs
   {
       public List<room_config> tables;

       public room_configs()
       {
           tables = new List<room_config>{
               new room_config( 8, 4, 1, 4 ),
               new room_config( 8, 4, 2, 1 ),
               new room_config( 8, 3, 1, 3 ),
               new room_config( 8, 3, 2, 1 ),
               new room_config( 8, 2, 1, 2 ),
               new room_config( 8, 2, 2, 1 ),
               new room_config( 16, 4, 1, 8 ),
               new room_config( 16, 4, 2, 2 ),
               new room_config( 16, 3, 1, 6 ),
               new room_config( 16, 3, 2, 2 ),
               new room_config( 16, 2, 1, 4 ),
               new room_config( 16, 2, 2, 2 )
           };
       }

       static private room_configs instance;
       static public room_configs GetInstance()
       {
           if (instance == null)
           {
               instance = new room_configs();
           }
           return instance;
       }
   }
}
