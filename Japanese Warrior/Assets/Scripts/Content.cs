using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Content {

    // Public getters, private setters
    public string contentID;
    public int contentID_OLD;
    public string language;
    public string contentType;
    public string japaneseContent;
    public string englishContent;

    // Public getters and setters
    public string AC;             // Acquisition Category
    public double AP;             // Acquisition Point
    public double AP_Multiplier;  // Acquisition Point Multiplier
    public int numberOfTest;       // How many times user tested itself?
    public int trueAnswers;       // How many times user answered true?

    
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
