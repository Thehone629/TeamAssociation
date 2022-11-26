"""
@FileName：user.py.py\n
@Description：\n
@Author：NZQ\n
@Time：2022/11/18 15:41\n
"""
from DBmodels.users import User
from flask import Blueprint
bp_user = Blueprint('user', __name__)


@bp_user.route("/user")
def user_demo():
    users = User()
    row = users.find_user_by_id('6')
    return row.name
