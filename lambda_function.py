import json

def lambda_handler(event, context):
    response = {
        "title": "some title",
        "content": "some content"
    }
    
    return {
        'statusCode': 200,
        'body': json.dumps(response),
        'headers': {
            'Content-Type': 'application/json'
        }
    }