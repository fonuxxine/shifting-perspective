from PIL import Image

def crop_transparent(image_path):
    # Open the image
    image = Image.open(image_path)

    # Get the alpha channel (transparency) if exists
    if image.mode in ('RGBA', 'LA') or (image.mode == 'P' and 'transparency' in image.info):
        alpha = image.convert('RGBA').split()[-1]

        # Get the bounding box of non-transparent pixels
        bbox = alpha.getbbox()

        # Crop the image with the bounding box
        cropped_image = image.crop(bbox)

        # Save the cropped image
        cropped_image.save("../Fairy_Cropped.png")
        print("Cropped image saved as 'cropped_" + image_path + "'")
    else:
        print("The image doesn't have a transparency channel.")

# Example usage
if __name__ == "__main__":
    image_path = "../Fairy.png"
    crop_transparent(image_path)
