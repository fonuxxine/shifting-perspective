from PIL import Image
# good in combination with: https://lospec.com/pixel-art-scaler/
def crop_center(image, new_width, new_height):
    width, height = image.size
    left = (width - new_width) / 2
    top = (height - new_height) / 2
    right = (width + new_width) / 2
    bottom = (height + new_height) / 2
    return image.crop((left, top, right, bottom))

def main():
    input_path = "XBOX_X.png"  # Path to your input PNG image
    output_path = "XBOX_X_Large.png"  # Path to save the cropped image

    # Open the image
    try:
        image = Image.open(input_path)
    except FileNotFoundError:
        print("Input file not found.")
        return
    except Exception as e:
        print("Error:", e)
        return

    # Crop the image to 784x784 while keeping it centered
    new_size = (784, 784)
    cropped_image = crop_center(image, *new_size)

    # Save the cropped image
    try:
        cropped_image.save(output_path)
        print("Cropped image saved successfully.")
    except Exception as e:
        print("Error saving cropped image:", e)

if __name__ == "__main__":
    main()