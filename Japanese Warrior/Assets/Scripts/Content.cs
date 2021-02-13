using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Content {
    
    // Public getters, private setters
    public int contentID { get; private set; }
    public string contentType { get; private set; }
    public string japaneseContent { get; private set; }
    public string englishContent { get; private set; }

    // Public getters and setters
    public int retentionRank { get; set; }
    public float retentionPoint { get; set; }
    // public int retentionClass { get; set; }          I disabled this field to keep it simple for now

    
    public Content(int contentID, string contentType, 
        string japaneseContent, string englishContent) {

        this.contentID = contentID;
        this.contentType = contentType;
        this.japaneseContent = japaneseContent;
        this.englishContent = englishContent;

        // Initialize the Retention System Parameters
        retentionRank = 1;
        retentionPoint = 1;
    }

}
