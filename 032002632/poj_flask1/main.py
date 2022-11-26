import os
from flask import Flask
from flask_sqlalchemy import SQLAlchemy
import config
from controller.user import  bp_user
from DBmodels.users import db



app = Flask(__name__,template_folder="template",static_url_path="/",static_folder="resource")
#设置盐值
app.config['SECRET_KEY'] = os.urandom(24)

app.config['SQLALCHEMY_DATABASE_URI'] = 'mysql+pymysql://root:123456@localhost:3306/test_py?charset=utf8'
#包裹app
db = SQLAlchemy(app)

from controller import *
app.register_blueprint(bp_user)


if __name__ == '__main__':
    app.run(debug=True,port=9090)






