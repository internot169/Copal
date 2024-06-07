from flask import Flask, request
import json

app = Flask(__name__)

@app.route("/")
def leaderboard():
    arr = []
    try:
        with open('scores.json', 'r') as f:
            arr = json.loads(f.read())

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
        arr = temp_arr
        #TODO: Sort by score and pick top 10
    except:
        print('Error reading scores.json')
    return arr

@app.route('/addscore', methods=['POST'])
def login():
    if(request.form['password'] == 'resin'):
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