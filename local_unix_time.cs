using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace local_unix_time
{
    class local_unix_time
    {
        static private class MyStr
        {
            public readonly static DateTime UnixEpoch_UTC   = new DateTime( 1970 , 1 , 1 , 0 , 0 , 0 , DateTimeKind.Utc );
            public readonly static DateTime UnixEpoch_Local = new DateTime( 1970 , 1 , 1 , 0 , 0 , 0 , DateTimeKind.Local );

            //
            //  文字列をUNIXタイムに変換する
            //  "2013-7-17 12:34:56" , "2013/7/17 12:34:56"
            static public long str2unixtime( string datetime_str )
            {
                string[] time_part = datetime_str.Replace( ":" , " " ).Replace( "-" , " " ).Replace( "/" , " " ).Split( ' ' );

                DateTime MyEpoch = new DateTime( int.Parse( time_part[ 0 ] ) , int.Parse( time_part[ 1 ] ) , int.Parse( time_part[ 2 ] ) ,
                                                 int.Parse( time_part[ 3 ] ) , int.Parse( time_part[ 4 ] ) , int.Parse( time_part[ 5 ] ) , DateTimeKind.Local );

                //return ( (long)MyEpoch.Subtract( UnixEpoch_Local ).TotalSeconds - 3600 * 9 );       //  ９時間調整
                return ( (long)MyEpoch.Subtract( UnixEpoch_Local ).TotalSeconds  );

            }

            //
            //  現在のUNIXタイムを取得する
            //
            static public long unixtime_now()
            {
               
                // return ( str2unixtime( DateTime.Now.ToLocalTime().ToString() ) );            //  こちらを選ぶと、９時間調整が必要になる。
                return ( str2unixtime( DateTime.Now.ToUniversalTime().ToString() ) );
            }
        }
        
        
        /// <summary>localtimeに関する基本的な試験</summary>
        static private void localtime_test()
        {
            Console.WriteLine( "{0}" , DateTime.Today );

            Console.WriteLine( "基準時間の表示" );
            Console.WriteLine( "UTC_string:{0}" , MyStr.UnixEpoch_UTC.ToString() );                     // 1970/01/01 0:00:00
            Console.WriteLine( "UTC_ToUniversalTime:{0}" , MyStr.UnixEpoch_UTC.ToUniversalTime() );     // 1970/01/01 0:00:00
            Console.WriteLine( "UTC_ToLocalTime:{0}" , MyStr.UnixEpoch_UTC.ToLocalTime() );             // 1970/01/01 0:00:00

            Console.WriteLine( "Local_string:{0}" , MyStr.UnixEpoch_Local.ToString() );                 // 1970/01/01 0:00:00
            Console.WriteLine( "Local_ToUniversalTime:{0}" , MyStr.UnixEpoch_Local.ToUniversalTime() ); // 1970/01/01 0:00:00
            Console.WriteLine( "Local_ToLocalTime:{0}" , MyStr.UnixEpoch_Local.ToLocalTime() );         // 1970/01/01 0:00:00

            Console.WriteLine( "現在時刻の表示" );

            Console.WriteLine( "unixtime_ToLocalTime(long):{0}" , DateTime.Now.ToLocalTime().ToString() );                // 2013/08/09 13:24:21
            Console.WriteLine( "unixtime_ToLocalTime(short):{0}" , DateTime.Now.ToLocalTime().ToShortTimeString() );      // 13:24

            Console.WriteLine( "unixtime1:{0}" , MyStr.str2unixtime( "2013-7-17 14:16:00" ) );          // 
            Console.WriteLine( "unixtime2:{0}" , MyStr.str2unixtime( "2013/7/17 14:16:00" ) );          // 
            Console.WriteLine( "unixtime-now:{0}" , MyStr.unixtime_now().ToString() );                  // 


        }

        static void Main( string[] args )
        {

            localtime_test();

            //  キー入力待ち
            ConsoleKeyInfo cki = Console.ReadKey();
        
        }
    }
}
