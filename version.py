import re
import codecs


def replace(fname, regexs):
    # Read in the file
    with codecs.open(fname, 'r', 'utf-8') as file:
        content = file.read()

    # Replace the target string
    if not isinstance(regexs, list):
        regexs = [regexs]

    for rFrom, rTo in regexs:
        content = re.sub(rFrom, rTo, content)

    # Write the file out again
    with codecs.open(fname, 'w', 'utf-8') as file:
        file.write(content)

    print('Replaced ' + fname)

version = input('Enter version:').strip()

if input('The version is {v}, are you sure? (y/n)'.format(v=version)) == 'y':
    replace('src/Properties/AssemblyInfo.cs', (r'Version\(".*"\)','Version("'+version+'")'))
    replace('src/FaceOnly.cs', (r'Version\(.*\)','Version('+version.replace('.',', ')+')'))
