using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DynamicBoard.Models
{
    public class Dataset
    {
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonProperty("x_axis_labels", NullValueHandling = NullValueHandling.Ignore)]
        public string? x_axis_labels { get; set; }

        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonProperty("label", NullValueHandling = NullValueHandling.Ignore)]
        public string? label { get; set; }


        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public List<int>? data { get; set; }


        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonProperty("fill", NullValueHandling = NullValueHandling.Ignore)]
        public bool? fill { get; set; }


        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonProperty("borderColor", NullValueHandling = NullValueHandling.Ignore)]
        public string? borderColor { get; set; }


        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonProperty("tension", NullValueHandling = NullValueHandling.Ignore)]
        public double? tension { get; set; }

        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonProperty("stack", NullValueHandling = NullValueHandling.Ignore)]
        public string? stack { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("type")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string? type { get; set; }

        [JsonProperty("backgroundColor", NullValueHandling = NullValueHandling.Ignore)]
        public string? backgroundColor { get; set; }


        // public List<string> backgroundColor { get; set; }
        // public List<string> borderColor { get; set; }
        //public string borderWidth { get; set; }
    }
    public class GraphConfiguration
    {

        //  public List<string> labels { get; set; }
        public List<Dataset> datasets { get; set; }
    }
}
