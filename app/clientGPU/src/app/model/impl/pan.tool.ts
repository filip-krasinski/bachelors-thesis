import { EditorComponent } from "../../components/editor/editor.component";
import { mat3, vec2 } from 'gl-matrix';
import { Camera } from '../camera';
import { CanvasPartComponent } from "../../components/canvas-part/canvas-part.component";
import { Tool } from "../interfaces";

export class PanTool extends Tool {
  private _dragging = false;

  private canvasPart!: CanvasPartComponent;
  private startInvViewProjMat = mat3.create();
  private startCamera!: Camera;
  private startPos = vec2.create();

  public onScroll = (event: WheelEvent) => {};

  public onMouseDown = (event: MouseEvent) => {
    const canvasPart = this.editor.getCanvasPartFromMousePosition(event.clientX, event.clientY);

    if (canvasPart != null) {
      this._dragging = true;
      this.canvasPart = canvasPart.instance;
      this.startCamera = Object.assign({}, this.canvasPart.camera);
      mat3.invert(this.startInvViewProjMat, this.canvasPart.camera.viewProjectionMat);
      const bbox = this.canvasPart.canvas?.nativeElement.getBoundingClientRect()!;
      const startclipvec = this.editor.getClipSpaceMousePositionVec2(event.clientX, event.clientY, bbox);
      vec2.transformMat3(this.startPos, startclipvec, this.startInvViewProjMat);
    }
  };

  public onMouseUp = (event: MouseEvent) => {
    if (this._dragging) {
      this._dragging = false;
    }
  };

  public onMouseMove = (event: MouseEvent) => {
    if (this._dragging && this.editor.context) {
      event.preventDefault();
      const pos = vec2.create();
      const bbox = this.canvasPart.canvas?.nativeElement.getBoundingClientRect()!;
      const mpPos = this.editor.getClipSpaceMousePositionVec2(event.clientX, event.clientY, bbox);
      vec2.transformMat3(pos, mpPos, this.startInvViewProjMat);

      this.canvasPart.camera.x = this.startCamera.x + this.startPos[0] - pos[0];
      this.canvasPart.camera.y = this.startCamera.y + this.startPos[1] - pos[1];
      this.editor.render(this.canvasPart);
    }
  };
}
