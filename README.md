## Description
The friend of the Simple Encoder. It can use basic statistical methods to analyze a given text to show whether it is likely to be encoded by only rail fence and/or Vigenère/Caesar cipher.
It first analyzes the likelihood of different combined key lengths = Vigenère key length * fence count.
Then it will automatically analyze the most likely key for each position in the key length but user-adjustable with a plot for likelihood reference. With a fence count f larger than 1, the yielded key would be an f-fold repetitive string so that the user will spot the presence of the fence cipher.

## To Use
- Input at the top textbox
- Use the Analyze button to find out the combined key length attempting the x range within the two numboxes next to the button. The plot right beneath it shows the likelihood of each key length.
- Use the W button to make everything lowercased and remove symbols and punctuations
- Click the Decipher button to find out the key at each position. The right numboxes means: total key length, current position, current position shift/offset (a=1, b=2, etc.)
