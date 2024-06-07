from flask import Flask, request
import json

app = Flask(__name__)

# Route to get the data for the leaderboard
@app.route("/")
def leaderboard():
    # No authentication necessary - this could be a public website
    arr = []
    # Try and except block to catch any errors that may occur when reading the file
    try:
        # Open the file and read the contents
        with open('scores.json', 'r') as f:
            arr = json.loads(f.read())

        # Convert the dictionary to a list of dictionaries with the name included
        temp_arr = []
        for i in arr:
            temp_arr.append({
                'name': i,
                'score': arr[i]['score'],
                'turns': arr[i]['turns'],
                'coins': arr[i]['coins'],
                'arrows': arr[i]['arrows'],
                'wumpus': arr[i]['wumpus']
            })
        # Sort the array in descending order by score
        temp_arr = sorted(temp_arr, key= lambda x: x['score'], reverse=True)
        # set the return array to this new array
        arr = temp_arr
    except:
        print('Error reading scores.json')
    return arr

# method to add scores to the leaderboard
@app.route('/addscore', methods=['POST'])
def addscore():
    # Not anyone can add scores - only the app
    # this is a rough way to do it - but this is a proof of concept
    if(request.form['password'] == 'resin'):
        # Try and except block to catch any errors that may occur when reading the file
        scores = {}
        try:
            with open('scores.json', 'r') as f:
                scores = json.load(f)
        except FileNotFoundError:
            pass
        
        # Overwriting the score if the name already exists
        # This makes it so each user can only set one high score
        scores[request.form['name']] = {
            'score': request.form['score'],
            'turns': request.form['turns'],
            'coins': request.form['coins'],
            'arrows': request.form['arrows'],
            'wumpus': request.form['wumpus']
        }
        
        # Save the scores to a file
        with open('scores.json', 'w') as f:
            json.dump(scores, f)
    
    return 'Score added'

if __name__ == '__main__':
    app.run()        