using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Global {
    namespace GlobalData {
        public static class GlobalData {
            public static List<DataHolder> datas = new List<DataHolder> ();




            public static DataObj Get (Key key) {
                return FindData (key)?.data;
            }

            public static DataHolder FindData (Key key) {
                DataHolder dataHolder;
                dataHolder = datas.Find ((x) => x.key == key);
                if (dataHolder == null) {
                    dataHolder = datas.Find ((data) => {
                        return data.key.Match (key);
                    });
                }
                return dataHolder;
            }



        }

        public class DataHolder {
            public Key key;
            public DataObj data;

        }
        public class DataObj {
            public System.Object obj;
            public int? intData;
            public string str;

            public DataObj (int intData) {
                this.intData = intData;
            }
            public void Set (int intData) {
                this.intData = intData;
            }
        }

        public class Key {
            public List<System.Object> objkeys = new List<System.Object> ();
            public List<string> strkeys = new List<string> ();

            public Key Add (System.Object obj) {
                objkeys.Add (obj);
                return this;
            }
            public Key Add (string str) {
                strkeys.Add (str);
                return this;
            }

            public bool Match (Key key) {
                bool result = true;
                key.objkeys.ForEach ((x) => {
                    if (!objkeys.Contains (x)) {
                        result = false;
                        return;
                    }
                });
                if (result == true) {
                    key.strkeys.ForEach ((x) => {
                        if (!strkeys.Contains (x)) {
                            result = false;
                            return;
                        }
                    });
                }
                return result;
            }

        }

    }



}