using System.Text.Json.Nodes;
using System;

namespace SAM.Core.IoT
{
    public class Record : IJSAMObject
    {
        private long ticks;
        private ParameterSet parameterSet;

        public Record(Record record)
        {
            if (record == null)
            {
                return;
            }

            ticks = record.ticks;
            parameterSet = record.parameterSet?.Clone();
        }

        public Record(DateTime dateTime)
        {
            ticks = dateTime.Ticks;
            parameterSet = new ParameterSet(string.Empty);
        }

        public bool Add(string name, double value)
        {
            return Add(name, value as object);
        }

        public bool Add(string name, string value)
        {
            return Add(name, value as object);
        }

        public bool Add(string name, int value)
        {
            return Add(name, value as object);
        }

        public bool Add(string name, bool value)
        {
            return Add(name, value as object);
        }

        public bool Add(string name, long value)
        {
            return Add(name, value as object);
        }

        public bool Add(string name, DateTime value)
        {
            return Add(name, value as object);
        }

        private bool Add(string name, object value)
        {
            if(name == null)
            {
                return false;
            }

            if(parameterSet == null)
            {
                parameterSet = new ParameterSet(string.Empty);
            }

            return parameterSet.Add(name , value as dynamic);
        }

        public bool FromJsonObject(JsonObject jObject)
        {
            if(jObject == null)
            {
                return false;
            }

            if(jObject.ContainsKey("Ticks"))
            {
                ticks = jObject["Ticks"]?.GetValue<long>() ?? default(long);
            }

            if (jObject.ContainsKey("ParameterSet"))
            {
                parameterSet = new ParameterSet(jObject["ParameterSet"] as JsonObject);
            }

            return true;
        }

        public JsonObject ToJsonObject()
        {
            JsonObject jObject = new JsonObject();
            jObject.Add("_type", Core.Query.FullTypeName(this));
            jObject.Add("Ticks", ticks);

            if (parameterSet != null)
                jObject.Add("ParameterSet", parameterSet.ToJsonObject());

            return jObject;
        }
    }
}
