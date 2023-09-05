# CwiczeniaNAI
Basics of AI - Practices from PJATK (4th semester)

On those lesson we've learned data science and AI creating.

Every package is dedicated to each topic:

  1) **KNN**
  
   Counts destination to every point, picks N number of nearest on based on their group desides the point group, thus being slow algorythm.
    
   ![image](https://github.com/LaneyBlack/CwiczeniaNAI/assets/44290162/e17bfe3b-f8e6-42f9-82c1-543ea74cb4f7)
    
  2) **KMeans**
  
   This algorythm is based on picking centroids. Random at first and then adjusting them using the training set. Faster than KNN, but not as reliable.
    
   ![image](https://github.com/LaneyBlack/CwiczeniaNAI/assets/44290162/c2195895-2d1f-451d-af07-d668e41eaa8c)

  3) **Perceptron** (Not only for this example)
  
   This algrythm is a lot closer to AI. On training set he is building coefficients of coordinates in different planes so that then, by substituting the coordinates of a point into the formula, decide in which group it belongs to.
    
   ![image](https://github.com/LaneyBlack/CwiczeniaNAI/assets/44290162/706b8ccd-6c2f-4db2-af2e-ea06194d1593)

  4) **LanguageAnalyzer**
  
   Pretty simple, but effective algorythm. In train set and test set we have texts in different languages. By counting the character frequency the algorythm builds a perceptron, that he later uses to decide the language of the text.
    
   ![image](https://github.com/LaneyBlack/CwiczeniaNAI/assets/44290162/163dc4bd-b302-40fa-800b-797e9bd39ccf)
    
   ![image](https://github.com/LaneyBlack/CwiczeniaNAI/assets/44290162/5d3fbbb3-8ae4-4850-bb52-9078a8f2e549)

  5) **Knapsack**
  
   This algorythm is trying to solve the Knapsack problem. Problem of putting **N** number of objects in **K** velocity backpack.
   I had 2 options of solving the problem. First reliable, but slow and second fast, but not as reliable.
    
   1) **Brute Force**
    
   This example had 32 items, thus counting all of them is a 6 minute work because of 2^32 possibilities to place all of the items in the backpack. So I decided to count them all using byte operations to increase algorythm efficiency.
    
   2) **Hill Climb**
    
   This algorythm pick a random solution and then tryes by walking through solution neighbours (where only 1 item has changed) climb to the best possible solution. This algorythm can be combined with multiple tries to get more reliable      results.
    
   ![image](https://github.com/LaneyBlack/CwiczeniaNAI/assets/44290162/76212e7d-ae85-4c30-a3ad-5bfa223a9347)


  
