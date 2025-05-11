using UnityEngine;

[System.Serializable]
public class Data 
{
    public string Nationality;
    public string Gender;

        public Data() {
        }

        public Data(string Nationality, string Gender) {
            this.Nationality = Nationality;
            this.Gender = Gender;
        }
}
