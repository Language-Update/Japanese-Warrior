﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;

public class Algorithm : MonoBehaviour{

	// Database stuff
	FirebaseManager FBmanager;

	// Trash
	List<Content> contents = new List<Content>();
	int userContentConunt_Native = 0;
	int userContentConunt_Perfect = 2;
	int userContentConunt_Good = 3;
	int userContentConunt_Moderate = 4;
	int userContentConunt_Weak = 6;
	int userContentConunt_New = 10;

	// Needed Categories
	int NC_Native = 0;  int NC_Perfect = 0; int NC_Good = 0; 
	int NC_Moderate = 0; int NC_Weak = 0; int NC_New = 0; int NC_Total = 0;


    // Here you can find how algorithm works by it's explaination, psuedo code, values, and actual code

    /*
	 * Content Properties  AP = Acquisition Point, AC = Acquisition Category
		=========================================================================================
		- Language: EN-JP
		- Content Type: Letter / Word / Phrase / Sentence ....
		- Content ID: 4481001245 (First 4 number is the country code on the languages. 44 for the UK, 81 for Japan)
		- AP = 1
		- AP Multiplier = 0.1
		- AC = "New"; (weak, moderate, good, perfect, native)
		- numberOfTest = 0; // This shows how many times user answered the question
		- (Gelecek) > Confuse <Dictinary>() (ContentID : ConfuseIndex)	// When user choose/select wrong content, we add that content to the confuse dic for the asked content. If there already is the confused content, we increase the index by 1)

		=========================================================================================

		Question Types and multipliers

		- Multiple Choice: 1 
		- Write:   1.5
		- Read:    1.3
		- Sorting: 1.1


		Check After

		- Native:   200 tests
		- Perfect:  100 tests
		- Good:     50  tests
		- Moderate: 25  tests
		- Weak:     12  tests


		Rate Of Repetation Type

		- New:		30%
		- Weak:		25%
		- Moderate:	20%
		- Good:		14%
		- Perfect:	8%
		- Native:	3%



		AC Options related to the number of tests

		if number of test >= *value* execute: if AP >= *value* : AC = *result*
		number of tests >= 200 : 10.000: Native, 2.000: Perfect, 500: Good, 100: Moderate, else Weak	| +%90-80-70-60-
		number of tests >= 100 : 700: Native, 210: Perfect, 90: Good, 28: Moderate, else Weak 		| +%95-80-70-60-
		number of tests >= 50  : 90: Perfect, 55: Good, 12: Moderate, else Weak				| +%90-80-60-
		number of tests >= 25  : 20: Good, 6: Moderate, else Weak					| +%80-60-
		number of tests >= 12  : 4: Moderate, else Weak							| +%60-
		number of tests >= 0   : New

		=========================================================================================

		Acquisition Point (AP) Algorithm

		- AP :1
		- AP Multiplier: 0.1 (increases .05 every time)
			10 True: 0.6
			20 True: 1.1
			30 True: 1.6
			40 True: 2.1

		After Question

		- True: 
			content.AP += questionMultiplier * 2^content.APmultiplier;
			content.APmultiplier += 0.1f;

		- False:
			content.AP -= questionMultiplier * 2^content.APmultiplier;
			content.APmultiplier -= 0.1f;



		=========================================================================================

		AC_Type_Selection_Method(int Total_Needed_Content)

		// Define how many content you need for each type

		Needed_Native_Count = Total_Needed_Content * (3/100);		// Needed_ContentType_Count's are class's own variables not methods
		Needed_Perfect_Count = Total_Needed_Content * (8/100);
		Needed_Good_Count = Total_Needed_Content * (14/100);
		Needed_Moderate_Count = Total_Needed_Content * (20/100);
		Needed_Weak_Count = Total_Needed_Content * (25/100);
		Needed_New_Count = Total_Needed_Content * (30/100);

		// If user has less then needed, then transfer the difference to the lower type and take what User's have

		if ( User's_Native_Count < Needed_Native_Count ) then			// 
			Needed_Perfect_Count += Needed_Native_Count - User's_Native_Count;
			Needed_Native_Count = User's_Native_Count;

		if ( User's_Perfect_Count < Needed_Perfect_Count ) then	
			Needed_Good_Count += Needed_Perfect_Count - User's_Perfect_Count;	// Transfer the difference
			Needed_Perfect_Count = User's_Perfect_Count;						// Take the remaining

		If ( ..... Same goes for other types: Good, Moderate, Weak, New);

		// Now you now how many content you can get for each type. If there are lots of content for every type which fulfills the request, then you will get as many as you defined initially. 
		// If there is no or not enough for the highest type, the difference between needed and current supply goes the lower type and you get what user have. 

		// For Example if algorithm need 5 Native, 12 Perfect, 20 Good, 35 Moderate, 45 Weak, 60 New content. If user only have 3 Native content, then algotihm pick all of those 3
			and transfer remainin 2 to Needed Perfect Content. So, Algorithm will look for 14 Perfect content instead of 12. If there is not enough of them, then same applies here again.
			Therefore algorithm will keep content rate constant as long as there is enough of them. The rate is defined above as "Rate Of Repetation Type"
	
	 
	********** NOTES **********
	*** Very Important: When you writing decimal numbers into the database, just use "."(point), don't use ","(comma) !!
	*** Very Important: When you writing number (int or double) be careful, it shouldn't be a string so database can sort it. 
		Check it when it writes. It should look like this: 5 / 5.5 not like "5" / "5.5" !!! 
	*
	  
	 
	 
	 
	 
	 
	 
	 
	 */


    private void Start() {
		FBmanager = FindObjectOfType<FirebaseManager>();
    }

    #region TEST Methods
    public void OpenOptionsAction() {
		//FBmanager.ReadUserData("", TestReading, "normal");
		//try { Debug.Log("Current User: " + FBmanager.GetUsername()); }
		//catch { Debug.LogError("No user logged-in !!!"); }

		//FBmanager.ReadNormalData("EN-JP/hiragana", TestOrderedAndFilteredReading, false);
				
		//	== All test done. Final Method:	
		// Bmanager.ReadedOrderedData("EN-JP/hiragana", TestDataManupulation, false, "AP");
		
	}

	void TestReading(DataSnapshot snapshot) {
		Debug.Log("All children data");
		foreach (DataSnapshot snap in snapshot.Children) {  // prints all the first layer children of the user
			Debug.Log("Snap Key: " + snap.Key + "    Value: " + snap.Value);
		}

		DataSnapshot profileSnapshot = snapshot.Child("profileInfo");
		Debug.Log("Is this first time in the app? --> " + profileSnapshot.Child("appFirstTime").Value); // prints yes
		Debug.Log("Is this first time in singleplayer mod? --> " + profileSnapshot.Child("singleFirstTime").Value); // prints no
	}

	void TestOrderedReading(DataSnapshot _snapshot) {
		Debug.Log("READ ordered data");

		foreach (DataSnapshot snapChild in _snapshot.Children) {
			Debug.Log("I get: " + snapChild.Child("content_EN").Value
				+ "   Its AP: " + snapChild.Child("AP").Value);

			/* This prints this:
             * I get: first_test   Its AP: 1
             * I get: second_test   Its AP: 2
             * I get: third_test   Its AP: 3
             * I get: 5_tes   Its AP: 5
             * I get: 10_test   Its AP: 10
             * 
             * So when we get snaphot as a whole, it has main children. When you get 1 child,
             * you need to go deeper. When you use .Child(key) you get that child. 
             * It may contain a value or children/child. So you get it's key and value seperately
             */
		}
	}

	void TestDataManupulation(DataSnapshot _snapshot) {
		Debug.Log("Testing Data manupulation");
		contents.Clear();   // Clear the list before using

		// ---------   Calculate Needed Content   --------- //
		/* TODO:						Status:
		 * * Execute NC_Definer			DONE
		 * 
		 */
		Debug.Log(""); Debug.Log("Defining Needed Contents"); Debug.Log("");
		// We should define how many content needed for each type when we want 5 content to ask
		TestDefineNeededCategories(5d);

		// ---------   Pick the needed content   --------- //
		/* TODO:																Status:
		 * - Check data's AC and is it needed? If so, add it to the list		DONE
		 * - Send it to the GM													DONE
		 */
		Debug.Log(""); Debug.Log("Picking needed contents"); Debug.Log("");
		foreach (DataSnapshot snapChild in _snapshot.Children) {

			// In here we're getting all the date from the data that we get from database. 
			//But we just need to take what we need and if we're done, we will break the loop.

			if (NC_Total <= 0) { break; } // if we're done. Then break.
			else {	// is that new? okay, and do I need a new? if so, get it. 
				if (snapChild.Child("AC").Value.ToString() == "New" && NC_New > 0) {
					Debug.Log("============== NEW ============== ");

					string contentID = "empty"; double contentAP = 0; string contentAC = "empty"; string contentEN = "empty";

					try { contentID = snapChild.Child("contentID").Value.ToString(); } catch { Debug.LogError(" Problem ID"); }
					// If I don't use .Replace(",",".") then it gets 1.5 and 1,5 wrong. Both wrong. 
					// I did use .Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture as a whole
					// Which Replaced commas with points (raplace) and then convert them reverse (InveriantCulture) LOL :D
					// So, just use double.Parse(string s);

					// And Firebase database only sorts 1.5 format. So, don't use "," when writing !!!
					try { contentAP = double.Parse(snapChild.Child("AP").Value.ToString()); } catch { Debug.LogError(" Problem AP"); }
					try { contentAC = snapChild.Child("AC").Value.ToString(); } catch { Debug.LogError(" Problem AC"); }
					try { contentEN = snapChild.Child("englishContent").Value.ToString(); } catch { Debug.LogError(" Problem content_EN"); }
					/*
					Debug.Log("Content ID (root): " + contentID + "  its type: " + contentID.GetType());
					Debug.Log("Content AC: " + contentAC + "  its type: " + contentAC.GetType());
					Debug.Log("Content AP: " + contentAP + "  its type: " + contentAP.GetType());
					Debug.Log("Content EN: " + contentEN + "  its type: " + contentEN.GetType());*/

					Debug.Log("Added: " + contentID + "-" + contentAP + "-" + contentAC + "-" + contentEN);
					contents.Add(new Content(contentID, contentAC, contentAP, contentEN));

					// Update Needed Contents
					NC_New--; NC_Total--;
				}
				else if (snapChild.Child("AC").Value.ToString() == "Weak" && NC_Weak > 0) {
					Debug.Log("============== NEW ============== ");

					string contentID = "empty"; double contentAP = 0; string contentAC = "empty"; string contentEN = "empty";

					try { contentID = snapChild.Child("contentID").Value.ToString(); } catch { Debug.LogError(" Problem ID"); }
					// If I don't use .Replace(",",".") then it gets 1.5 and 1,5 wrong. Both wrong. 
					// I did use .Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture as a whole
					// Which Replaced commas with points (raplace) and then convert them reverse (InveriantCulture) LOL :D
					// So, just use double.Parse(string s);

					// And Firebase database only sorts 1.5 format. So, don't use "," when writing !!!
					try { contentAP = double.Parse(snapChild.Child("AP").Value.ToString()); } catch { Debug.LogError(" Problem AP"); }
					try { contentAC = snapChild.Child("AC").Value.ToString(); } catch { Debug.LogError(" Problem AC"); }
					try { contentEN = snapChild.Child("englishContent").Value.ToString(); } catch { Debug.LogError(" Problem content_EN"); }
					/*
					Debug.Log("Content ID (root): " + contentID + "  its type: " + contentID.GetType());
					Debug.Log("Content AC: " + contentAC + "  its type: " + contentAC.GetType());
					Debug.Log("Content AP: " + contentAP + "  its type: " + contentAP.GetType());
					Debug.Log("Content EN: " + contentEN + "  its type: " + contentEN.GetType());*/

					Debug.Log("Added: " + contentID + "-" + contentAP + "-" + contentAC + "-" + contentEN);
					contents.Add(new Content(contentID, contentAC, contentAP, contentEN));

					// Update Needed Contents
					NC_Weak--; NC_Total--;
				}
				else if (snapChild.Child("AC").Value.ToString() == "Moderate" && NC_Moderate > 0) {
					Debug.Log("============== NEW ============== ");

					string contentID = "empty"; double contentAP = 0; string contentAC = "empty"; string contentEN = "empty";

					try { contentID = snapChild.Child("contentID").Value.ToString(); } catch { Debug.LogError(" Problem ID"); }
					// If I don't use .Replace(",",".") then it gets 1.5 and 1,5 wrong. Both wrong. 
					// I did use .Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture as a whole
					// Which Replaced commas with points (raplace) and then convert them reverse (InveriantCulture) LOL :D
					// So, just use double.Parse(string s);

					// And Firebase database only sorts 1.5 format. So, don't use "," when writing !!!
					try { contentAP = double.Parse(snapChild.Child("AP").Value.ToString()); } catch { Debug.LogError(" Problem AP"); }
					try { contentAC = snapChild.Child("AC").Value.ToString(); } catch { Debug.LogError(" Problem AC"); }
					try { contentEN = snapChild.Child("englishContent").Value.ToString(); } catch { Debug.LogError(" Problem content_EN"); }
					/*
					Debug.Log("Content ID (root): " + contentID + "  its type: " + contentID.GetType());
					Debug.Log("Content AC: " + contentAC + "  its type: " + contentAC.GetType());
					Debug.Log("Content AP: " + contentAP + "  its type: " + contentAP.GetType());
					Debug.Log("Content EN: " + contentEN + "  its type: " + contentEN.GetType());*/

					Debug.Log("Added: " + contentID + "-" + contentAP + "-" + contentAC + "-" + contentEN);
					contents.Add(new Content(contentID, contentAC, contentAP, contentEN));

					// Update Needed Contents
					NC_Moderate--; NC_Total--;
				}
				else if (snapChild.Child("AC").Value.ToString() == "Good" && NC_Good > 0) {
					Debug.Log("============== NEW ============== ");

					string contentID = "empty"; double contentAP = 0; string contentAC = "empty"; string contentEN = "empty";

					try { contentID = snapChild.Child("contentID").Value.ToString(); } catch { Debug.LogError(" Problem ID"); }
					// If I don't use .Replace(",",".") then it gets 1.5 and 1,5 wrong. Both wrong. 
					// I did use .Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture as a whole
					// Which Replaced commas with points (raplace) and then convert them reverse (InveriantCulture) LOL :D
					// So, just use double.Parse(string s);

					// And Firebase database only sorts 1.5 format. So, don't use "," when writing !!!
					try { contentAP = double.Parse(snapChild.Child("AP").Value.ToString()); } catch { Debug.LogError(" Problem AP"); }
					try { contentAC = snapChild.Child("AC").Value.ToString(); } catch { Debug.LogError(" Problem AC"); }
					try { contentEN = snapChild.Child("englishContent").Value.ToString(); } catch { Debug.LogError(" Problem content_EN"); }
					/*
					Debug.Log("Content ID (root): " + contentID + "  its type: " + contentID.GetType());
					Debug.Log("Content AC: " + contentAC + "  its type: " + contentAC.GetType());
					Debug.Log("Content AP: " + contentAP + "  its type: " + contentAP.GetType());
					Debug.Log("Content EN: " + contentEN + "  its type: " + contentEN.GetType());*/

					Debug.Log("Added: " + contentID + "-" + contentAP + "-" + contentAC + "-" + contentEN);
					contents.Add(new Content(contentID, contentAC, contentAP, contentEN));

					// Update Needed Contents
					NC_Good--; NC_Total--;
				}
				else if (snapChild.Child("AC").Value.ToString() == "Perfect" && NC_Perfect > 0) {
					Debug.Log("============== NEW ============== ");

					string contentID = "empty"; double contentAP = 0; string contentAC = "empty"; string contentEN = "empty";

					try { contentID = snapChild.Child("contentID").Value.ToString(); } catch { Debug.LogError(" Problem ID"); }
					// If I don't use .Replace(",",".") then it gets 1.5 and 1,5 wrong. Both wrong. 
					// I did use .Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture as a whole
					// Which Replaced commas with points (raplace) and then convert them reverse (InveriantCulture) LOL :D
					// So, just use double.Parse(string s);

					// And Firebase database only sorts 1.5 format. So, don't use "," when writing !!!
					try { contentAP = double.Parse(snapChild.Child("AP").Value.ToString()); } catch { Debug.LogError(" Problem AP"); }
					try { contentAC = snapChild.Child("AC").Value.ToString(); } catch { Debug.LogError(" Problem AC"); }
					try { contentEN = snapChild.Child("englishContent").Value.ToString(); } catch { Debug.LogError(" Problem content_EN"); }
					/*
					Debug.Log("Content ID (root): " + contentID + "  its type: " + contentID.GetType());
					Debug.Log("Content AC: " + contentAC + "  its type: " + contentAC.GetType());
					Debug.Log("Content AP: " + contentAP + "  its type: " + contentAP.GetType());
					Debug.Log("Content EN: " + contentEN + "  its type: " + contentEN.GetType());*/

					Debug.Log("Added: " + contentID + "-" + contentAP + "-" + contentAC + "-" + contentEN);
					contents.Add(new Content(contentID, contentAC, contentAP, contentEN));

					// Update Needed Contents
					NC_Perfect--; NC_Total--;
				}
				else if (snapChild.Child("AC").Value.ToString() == "Native" && NC_Native > 0) {
					Debug.Log("============== NEW ============== ");

					string contentID = "empty"; double contentAP = 0; string contentAC = "empty"; string contentEN = "empty";

					try { contentID = snapChild.Child("contentID").Value.ToString(); } catch { Debug.LogError(" Problem ID"); }
					// If I don't use .Replace(",",".") then it gets 1.5 and 1,5 wrong. Both wrong. 
					// I did use .Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture as a whole
					// Which Replaced commas with points (raplace) and then convert them reverse (InveriantCulture) LOL :D
					// So, just use double.Parse(string s);

					// And Firebase database only sorts 1.5 format. So, don't use "," when writing !!!
					try { contentAP = double.Parse(snapChild.Child("AP").Value.ToString()); } catch { Debug.LogError(" Problem AP"); }
					try { contentAC = snapChild.Child("AC").Value.ToString(); } catch { Debug.LogError(" Problem AC"); }
					try { contentEN = snapChild.Child("englishContent").Value.ToString(); } catch { Debug.LogError(" Problem content_EN"); }
					/*
					Debug.Log("Content ID (root): " + contentID + "  its type: " + contentID.GetType());
					Debug.Log("Content AC: " + contentAC + "  its type: " + contentAC.GetType());
					Debug.Log("Content AP: " + contentAP + "  its type: " + contentAP.GetType());
					Debug.Log("Content EN: " + contentEN + "  its type: " + contentEN.GetType());*/

					Debug.Log("Added: " + contentID + "-" + contentAP + "-" + contentAC + "-" + contentEN);
					contents.Add(new Content(contentID, contentAC, contentAP, contentEN));

					// Update Needed Contents
					NC_Native--; NC_Total--;
				}
			}
		}
		Debug.Log(""); Debug.Log("Contents list created"); Debug.Log("");
		Debug.Log("Content Amount: " + contents.Count);
		/*foreach (Content content in contents) {
			Debug.Log("Added Content: " + content.englishContent);
        }*/
		// Now we get all the data we want in a list mixed. 

		/*	We should send it to the game manager. Game Manager sends it to the Question Handler.
			Question handler asks and keep records true answers. 
			Then it will pass the final type of them to us via Game Manager.
			We (algorithm) will execute AP Calculation and AC identifier to calculte new AP and
				define AC for all. If there is any change, we will record it to the StatusInfo in the database

			When user wants to see it's data
		*/

		// ---------   Manupulate Data   --------- //
		/* TODO:						Status:
		 * - Increse true answers		DONE
		 * - Calculate AP				DONE
		 * - Identify AC				SKIPPED
		 */
		Debug.Log(""); Debug.Log("Data manupulation starts"); Debug.Log("");

		// Increasing True Answers
		System.Random rnd = new System.Random();
		foreach (Content content in contents) {
			int temp = content.trueAnswers;
			content.trueAnswers = rnd.Next(10);
			Debug.Log("Content: " + content.englishContent);
			Debug.Log("Changed: " + temp + " --> " + content.trueAnswers);
        }

		// Calculate AP
		foreach (Content content in contents) {
			double initialAP = content.AP;	int initialTrue = content.trueAnswers;
			while (content.trueAnswers > 0) {
				content.AP += Mathf.Pow(2, (float)content.AP_Multiplier);	// Add new point
				content.AP_Multiplier += 0.1; // It will be 0.05			// Increase point multiplier
				content.trueAnswers--;										// Decrease True Answer
            }
			Debug.Log("Change On: " + content.englishContent + " with " + initialTrue + " amount of True."
				+ "    AP: " + initialAP + " --> " + content.AP);
        }

		// Identify AC	** NO NEED TO TEST **
		foreach (Content content in contents) {
			string initialAC = content.AC;

			
        }



		Debug.Log(""); Debug.Log("Update Data"); Debug.Log("");
		// ---------   Update Data   --------- //
		/* TODO:						Status:
		 * - Record Changes				SKIPPED
		 * - Update All Data			DONE
		 */

		// Update All Data
		
		// Create 2D array which will carry Json data and the path to write it
		
		string[,] DataToUpdate = new string[contents.Count, contents.Count];
		
		for (int i = 0; i < contents.Count; i++) {
			DataToUpdate[0, i] = "EN-JP/hiragana/" + contents[i].contentID;	// Address
			DataToUpdate[1, i] = JsonUtility.ToJson(contents[i]);			// Value
        }

		FBmanager.WriteJsonData(DataToUpdate);
		/*
		string address = "EN-JP/hiragana/" + contents[0].contentID;
		string json = JsonUtility.ToJson(contents[0]);
		Debug.Log("Json test - " + contents[0].AP.ToString());
		Debug.Log("JSON ?? - " + json); ;
		FBmanager.SingleJson(address, json);*/

	}

	void TestDefineNeededCategories(double TotalNeededContent) {
		// Define how many content you need for each type
		NC_Native = (int)(TotalNeededContent * (3d / 100d)) + 1;	//Debug.Log("---- IN ---- Native: " + NC_Native);
		NC_Perfect = (int)(TotalNeededContent * (8d / 100d)) + 1;// Debug.Log("---- IN ---- Perfect: " + NC_Perfect);
		NC_Good = (int)(TotalNeededContent * (14d / 100d)) + 1; //Debug.Log("---- IN ---- Good: " + NC_Good);
		NC_Moderate = (int)(TotalNeededContent * (20d / 100d)) + 1; //Debug.Log("---- IN ---- Moderate: " + NC_Moderate);
		NC_Weak = (int)(TotalNeededContent * (25d / 100d)) + 1; //Debug.Log("---- IN ---- Weak: " + NC_Weak);
		NC_New = (int)(TotalNeededContent * (30d / 100d)) + 1; //Debug.Log("---- IN ---- New: " + NC_New);

		// If user has less then needed, then transfer the difference to the lower type and take what User's have

		if (userContentConunt_Native < NC_Native ) {
			NC_Perfect += NC_Native - userContentConunt_Native;	
			//Debug.Log(NC_Native - userContentConunt_Native + " goes to Perfect!" + " - Now Perfect: " + NC_Perfect);
			NC_Native = userContentConunt_Native; //Debug.Log(NC_Native + " amount left in Native !");
        }
		if (userContentConunt_Perfect < NC_Perfect) {
			NC_Good += NC_Perfect - userContentConunt_Perfect; 
			//Debug.Log(NC_Perfect - userContentConunt_Perfect + " goes to Good!" + " - Now Good: " + NC_Good);
			NC_Perfect = userContentConunt_Perfect; //Debug.Log(NC_Perfect + " amount left in Perfect !");
		}
		if (userContentConunt_Good < NC_Good) {
			NC_Moderate += NC_Good - userContentConunt_Good; 
			//Debug.Log(NC_Good - userContentConunt_Good + " goes to Moderate!" + " - Now Moderate: " + NC_Moderate);
			NC_Good = userContentConunt_Good; //Debug.Log(NC_Good + " amount left in Good !");
		}
		if (userContentConunt_Moderate < NC_Moderate) {
			NC_Weak += NC_Moderate - userContentConunt_Moderate; 
			//Debug.Log(NC_Moderate - userContentConunt_Moderate + " goes to Weak!" + " - Now Weak: " + NC_Weak);
			NC_Moderate = userContentConunt_Moderate;// Debug.Log(NC_Moderate + " amount left in Moderate !");
		}
		if (userContentConunt_Weak < NC_Weak) {
			NC_New += NC_Weak - userContentConunt_Weak; 
			//Debug.Log(NC_Weak - userContentConunt_Weak + " goes to New!" + " - Now New: " + NC_New);
			NC_Weak = userContentConunt_Weak; //Debug.Log(NC_Weak + " amount left in Weak !");
		}
		// Define how many content you need for each type
		/*Debug.Log("---- OUT ---- Native: " + NC_Native);
		Debug.Log("---- OUT ---- Perfect: " + NC_Perfect);
		Debug.Log("---- OUT ---- Good: " + NC_Good);
		Debug.Log("---- OUT ---- Moderate: " + NC_Moderate);
		Debug.Log("---- OUT ---- Weak: " + NC_Weak);
		Debug.Log("---- OUT ---- New: " + NC_New);*/

		// Keep total number of needed
		NC_Total = NC_Native + NC_Perfect + NC_Good + NC_Moderate + NC_Weak + NC_New;
		//Debug.Log("Total returned content: " + NC_Total);
	}



    #endregion





}
