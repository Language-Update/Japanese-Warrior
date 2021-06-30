using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Content {
    
    // Public getters, private setters
    public string contentID { get; private set; }
    public int contentID_OLD { get; private set; }
    public string language { get; private set; }
    public string contentType { get; private set; }
    public string japaneseContent { get; private set; }
    public string englishContent { get; private set; }

    // Public getters and setters
    public string AC { get; set; }              // Acquisition Category
    public double AP { get; set; }              // Acquisition Point
    public double AP_Multiplier { get; set; }   // Acquisition Point Multiplier
    public int numberOfTest { get; set; }       // How many times user tested itself?
    public int trueAnswers { get; set; }        // How many times user answered true?

    
    public Content(string _contentID, string _AC, double _AP, string _englishContent) {

        contentID = _contentID;
        AC = _AC;
        AP = _AP;
        englishContent = _englishContent;

        trueAnswers = 0;
        AP_Multiplier = 0.1;
        /*
        // Initialize the Retention System Parameters
        retentionRank = 1;
        retentionPoint = 1;*/
    }

    public Content(int _contentID, string _contentType, string _japaneseContent, string _englishContent) {

        this.contentID_OLD -= _contentID;
        this.contentType = _contentType;
        this.japaneseContent = _japaneseContent;
        this.englishContent = _englishContent;

    }

}
