/*this caller file is codegen by meter for c#*/
using System;
using System.Collections.Generic;

namespace meter
{
   public class rate
   {
       public float rate_rmb;

       public rate( float _rate_rmb )
       {
           rate_rmb = _rate_rmb;
       }
   }

   public class rates
   {
       public List<rate> tables;

       public rates()
       {
           tables = new List<rate>{
               new rate( (float)0.01 ),
               new rate( (float)0.012 ),
               new rate( (float)0.013 ),
               new rate( (float)0.014 )
           };
       }

       static private rates instance;
       static public rates GetInstance()
       {
           if (instance == null)
           {
               instance = new rates();
           }
           return instance;
       }
        
   }
}
