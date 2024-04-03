from PIL import Image
import os


def replace_white_with_transparency(image):
    img = image.convert("RGBA")
    datas = img.getdata()
    newData = []
    for item in datas:
        # replace white pixels with transparency
        if item[0] == 255 and item[1] == 255 and item[2] == 255:
            newData.append((255, 255, 255, 0))
        else:
            newData.append(item)
    img.putdata(newData)
    return img


def process_images_in_folder(folder_path):
    for filename in os.listdir(folder_path):
        if filename.endswith(".png"):
            image_path = os.path.join(folder_path, filename)
            image = Image.open(image_path)
            new_image = replace_white_with_transparency(image)
            new_image.save(os.path.join(folder_path, filename))


if __name__ == "__main__":
    folder_path = os.path.dirname(os.path.abspath(__file__))
    process_images_in_folder(folder_path)
