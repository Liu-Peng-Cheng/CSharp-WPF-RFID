using System;
using System.Collections.Generic;
using System.Text;

namespace PublicAPI.CKC001.MessageObj
{
    public class Tags
    {
        public int all_tag_num;
        public int add_tag_num;
        public int loss_tag_num;
        public List<_tag> add_tag_list;
        public List<_tag> loss_tag_list;
        public List<_tag> all_tag_list;
    }
    public class _tag
    {
        public string epc;
    }
}
