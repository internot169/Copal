from markupsafe import escape
from flask import Flask, request, render_template
import json

app = Flask(__name__)

@app.route("/")
def leaderboard():
    scores = {}
    try:
        with open('scores.json', 'r') as f:
            scores = json.load(f)
        arr = []

        for key in scores:
            arr.append({'name': scores[key]['name'], 
                        'score': scores[key]['score'], 
                        'turns': scores[key]['turns'], 
                        'coins': scores[key]['coins'], 
                        'arrows': scores[key]['arrows'], 
                        'wumpus': scores[key]['wumpus']})
            
        return render_template('index.html', scores=arr)
    except:
        return render_template('index.html', scores=[])

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