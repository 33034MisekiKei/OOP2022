﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise {
    //クラス
    class Song {
        //歌のタイトル
        public string Title { get; set; }

        //アーティスト名
        public string ArtistName { get; set; }

        //演奏時間、単位は秒
        public int Length { get; set; }

        //引数付きコンストラクタ
        public Song(string title, string artistname, int length) {
            Title = title;
            ArtistName = artistname;
            Length = length;
        }
    }
}
