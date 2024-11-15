import json

def lambda_handler(event, context):
    # Define variables
    statusCode = 200

    # Access query parameters
    query_params = event.get('queryStringParameters', {})

    # Predefined parameters with default values
    predefined_params = {
        "gameId": ""
        }

    # Update predefined_params with actual query parameters if available
    predefined_params.update({k: v for k, v in query_params.items() if k in predefined_params})

    # Validate query parameters
    if predefined_params['gameId'] == "":
        statusCode = 400
        response = "Invalid value for 'gameId'"
    else:
        response = {
            "game-id": predefined_params['gameId'],
            "source": "Steam",
            "title": "Valve giveth, and Valve taketh away: Team Fortress 2's BLU Scout is once again wearing the 'wrong' pants after a 17 years-in-the-making fix was reversed a day later",
            "contents": "Signs of life from Team Fortress 2 are rare and precious these days, so when Valve updated the in-game model of the Scout, \u003Ca href=\"https://www.pcgamer.com/games/fps/17-years-later-valve-fixes-team-fortress-2-bug-that-made-scouts-pants-the-wrong-color-bug-so-old-it-could-have-enlisted-with-parental-consent/\" target=\"_blank\"\u003Efixing a visual bug that's existed since TF2's release\u003C/a\u003E in 2007, we were paying attention. But as reported by YouTuber \u003Ca href=\"https://www.youtube.com/watch?v=jS9R32G2-lo&ab_channel=shounic\" target=\"_blank\"\u003Eshounic\u003C/a\u003E, the visual tweak was not to last: A \u003Ca href=\"https://store.steampowered.com/news/app/440/view/4520017657931497816?l=english\" target=\"_blank\"\u003Efollow-up patch\u003C/a\u003E has reverted the BLU Scout's pants to their original, incorrect khaki from a smart, team-appropriate navy...",
            "read-more": 'https://www.pcgamer.com/games/fps/valve-giveth-and-valve-taketh-away-team-fortress-2s-blu-scout-is-once-again-wearing-the-wrong-pants-after-a-17-years-in-the-making-fix-was-reversed-a-day-later?utm_source=steam&utm_medium=referral\\'
            }

    return {
        'statusCode': statusCode,
        'body': json.dumps(response),
        'headers': {
            'Access-Control-Allow-Headers': 'Content-Type',
            'Access-Control-Allow-Origin': '*',
            'Access-Control-Allow-Methods': 'GET, OPTIONS',
            'Access-Control-Max-Age': '60',
            'Content-Type': 'application/json'
            }
        }
